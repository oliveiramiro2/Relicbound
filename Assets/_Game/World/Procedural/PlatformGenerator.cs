using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator
{
    private readonly GameObject platformPrefab;

    private readonly Vector2 platformSize;

    private readonly ReachabilityValidator
        reachabilityValidator;

    public PlatformGenerator(
        GameObject platformPrefab
    )
    {
        this.platformPrefab =
            platformPrefab;

        platformSize =
            Vector2.zero;

        reachabilityValidator =
            new ReachabilityValidator();

        if (platformPrefab == null)
            return;

        ProceduralPlatform platform =
            platformPrefab.GetComponent<
                ProceduralPlatform>();

        if (platform == null)
            return;

        platformSize =
            platform.Size;
    }

    public PlatformGenerationResult Generate(
        RoomGenerationBounds bounds,
        System.Random random,
        Transform parent,
        int count,
        float minimumSpacing,
        MovementReachProfile reachProfile,
        int maximumAttemptsPerPlatform,
        int minimumBranches,
        int maximumBranches,
        int minimumBranchLength,
        int maximumBranchLength
    )
    {
        PlatformGraph graph =
            new PlatformGraph();

        List<GeneratedPlatform>
            mainPathPlatforms =
                new List<GeneratedPlatform>();

        List<GeneratedPlatform>
            branchPlatforms =
                new List<GeneratedPlatform>();

        List<GeneratedPlatform>
            deadEndPlatforms =
                new List<GeneratedPlatform>();

        if (!ValidateInput(
                bounds,
                random,
                parent,
                count,
                maximumAttemptsPerPlatform,
                reachProfile))
        {
            return CreateResult(
                graph,
                null,
                null,
                mainPathPlatforms,
                branchPlatforms,
                deadEndPlatforms
            );
        }

        if (!HasValidPlatformSize())
        {
            return CreateResult(
                graph,
                null,
                null,
                mainPathPlatforms,
                branchPlatforms,
                deadEndPlatforms
            );
        }

        CalculateGenerationLimits(
            bounds,
            out float minX,
            out float maxX,
            out float minY,
            out float maxY
        );

        if (minX > maxX ||
            minY > maxY)
        {
            return CreateResult(
                graph,
                null,
                null,
                mainPathPlatforms,
                branchPlatforms,
                deadEndPlatforms
            );
        }

        List<Rect> occupiedAreas =
            new List<Rect>();

        GeneratedPlatform startPlatform =
            GenerateStartPlatform(
                minX,
                maxX,
                minY,
                random,
                parent,
                graph,
                occupiedAreas
            );

        if (startPlatform == null)
        {
            return CreateResult(
                graph,
                null,
                null,
                mainPathPlatforms,
                branchPlatforms,
                deadEndPlatforms
            );
        }

        mainPathPlatforms.Add(
            startPlatform
        );

        GeneratedPlatform previousPlatform =
            startPlatform;

        for (
            int index = 1;
            index < count;
            index++)
        {
            bool isExit =
                index == count - 1;

            GeneratedPlatform nextPlatform =
                TryGeneratePlatform(
                    previousPlatform,
                    index,
                    isExit,
                    PlatformRouteType.MainPath,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    parent,
                    graph,
                    occupiedAreas,
                    minimumSpacing,
                    reachProfile,
                    maximumAttemptsPerPlatform
                );

            if (nextPlatform == null)
            {
                break;
            }

            ReachabilityCheckResult
                reachability =
                reachabilityValidator.Check(
                    previousPlatform,
                    nextPlatform,
                    reachProfile
                );

            if (!reachability.IsReachable)
            {
                break;
            }

            graph.Connect(
                previousPlatform,
                nextPlatform,
                reachability.Type
            );

            mainPathPlatforms.Add(
                nextPlatform
            );

            previousPlatform =
                nextPlatform;
        }

        GeneratedPlatform exitPlatform =
            previousPlatform.IsExit
                ? previousPlatform
                : null;

        if (exitPlatform == null)
        {
            return CreateResult(
                graph,
                startPlatform,
                null,
                mainPathPlatforms,
                branchPlatforms,
                deadEndPlatforms
            );
        }

        int branchCount =
            CalculateBranchCount(
                random,
                minimumBranches,
                maximumBranches
            );

        GenerateBranches(
            branchCount,
            mainPathPlatforms,
            minX,
            maxX,
            minY,
            maxY,
            random,
            parent,
            graph,
            occupiedAreas,
            minimumSpacing,
            reachProfile,
            maximumAttemptsPerPlatform,
            minimumBranchLength,
            maximumBranchLength,
            branchPlatforms,
            deadEndPlatforms
        );

        return CreateResult(
            graph,
            startPlatform,
            exitPlatform,
            mainPathPlatforms,
            branchPlatforms,
            deadEndPlatforms
        );
    }

    private void GenerateBranches(
        int branchCount,
        List<GeneratedPlatform> mainPathPlatforms,
        float minX,
        float maxX,
        float minY,
        float maxY,
        System.Random random,
        Transform parent,
        PlatformGraph graph,
        List<Rect> occupiedAreas,
        float minimumSpacing,
        MovementReachProfile reachProfile,
        int maximumAttemptsPerPlatform,
        int minimumBranchLength,
        int maximumBranchLength,
        List<GeneratedPlatform> branchPlatforms,
        List<GeneratedPlatform> deadEndPlatforms
    )
    {
        if (branchCount <= 0)
            return;

        if (mainPathPlatforms.Count < 2)
            return;

        List<GeneratedPlatform>
            availableParents =
            new List<GeneratedPlatform>(
                mainPathPlatforms
            );

        Shuffle(
            availableParents,
            random
        );

        int generatedBranches = 0;

        foreach (
            GeneratedPlatform branchParent
            in availableParents)
        {
            if (generatedBranches >=
                branchCount)
            {
                break;
            }

            if (branchParent.IsStart)
                continue;

            if (branchParent.IsExit)
                continue;

            int branchLength =
                random.Next(
                    minimumBranchLength,
                    maximumBranchLength + 1
                );

            List<GeneratedPlatform>
                generatedBranch =
                GenerateBranch(
                    branchParent,
                    branchLength,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    parent,
                    graph,
                    occupiedAreas,
                    minimumSpacing,
                    reachProfile,
                    maximumAttemptsPerPlatform,
                    branchPlatforms
                );

            if (generatedBranch.Count == 0)
                continue;

            generatedBranches++;

            GeneratedPlatform lastPlatform =
                generatedBranch[
                    generatedBranch.Count - 1
                ];

            lastPlatform =
                MarkAsDeadEnd(
                    lastPlatform
                );

            deadEndPlatforms.Add(
                lastPlatform
            );
        }
    }

    private List<GeneratedPlatform>
        GenerateBranch(
            GeneratedPlatform parentPlatform,
            int length,
            float minX,
            float maxX,
            float minY,
            float maxY,
            System.Random random,
            Transform parent,
            PlatformGraph graph,
            List<Rect> occupiedAreas,
            float minimumSpacing,
            MovementReachProfile reachProfile,
            int maximumAttemptsPerPlatform,
            List<GeneratedPlatform> branchPlatforms
        )
    {
        List<GeneratedPlatform>
            generatedPlatforms =
            new List<GeneratedPlatform>();

        GeneratedPlatform previousPlatform =
            parentPlatform;

        for (
            int index = 0;
            index < length;
            index++)
        {
            GeneratedPlatform branchPlatform =
                TryGeneratePlatform(
                    previousPlatform,
                    graph.Platforms.Count,
                    false,
                    PlatformRouteType.Branch,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    parent,
                    graph,
                    occupiedAreas,
                    minimumSpacing,
                    reachProfile,
                    maximumAttemptsPerPlatform
                );

            if (branchPlatform == null)
            {
                break;
            }

            ReachabilityCheckResult
                reachability =
                reachabilityValidator.Check(
                    previousPlatform,
                    branchPlatform,
                    reachProfile
                );

            if (!reachability.IsReachable)
            {
                break;
            }

            graph.Connect(
                previousPlatform,
                branchPlatform,
                reachability.Type
            );

            generatedPlatforms.Add(
                branchPlatform
            );

            branchPlatforms.Add(
                branchPlatform
            );

            previousPlatform =
                branchPlatform;
        }

        return generatedPlatforms;
    }

    private GeneratedPlatform MarkAsDeadEnd(
    GeneratedPlatform platform
)
    {
        return platform;
    }

    private int CalculateBranchCount(
        System.Random random,
        int minimumBranches,
        int maximumBranches
    )
    {
        minimumBranches =
            Mathf.Max(
                0,
                minimumBranches
            );

        maximumBranches =
            Mathf.Max(
                minimumBranches,
                maximumBranches
            );

        return random.Next(
            minimumBranches,
            maximumBranches + 1
        );
    }

    private void Shuffle<T>(
        List<T> list,
        System.Random random
    )
    {
        for (
            int i = list.Count - 1;
            i > 0;
            i--)
        {
            int index =
                random.Next(
                    i + 1
                );

            T temporary =
                list[i];

            list[i] =
                list[index];

            list[index] =
                temporary;
        }
    }

    private GeneratedPlatform
        GenerateStartPlatform(
            float minX,
            float maxX,
            float minY,
            System.Random random,
            Transform parent,
            PlatformGraph graph,
            List<Rect> occupiedAreas
        )
    {
        float x =
            Mathf.Lerp(
                minX,
                maxX,
                (float)random.NextDouble()
            );

        Vector2 position =
            new Vector2(
                x,
                minY
            );

        return CreatePlatform(
            position,
            0,
            true,
            false,
            PlatformRouteType.MainPath,
            parent,
            graph,
            occupiedAreas
        );
    }

    private GeneratedPlatform
        TryGeneratePlatform(
            GeneratedPlatform previousPlatform,
            int index,
            bool isExit,
            PlatformRouteType routeType,
            float minX,
            float maxX,
            float minY,
            float maxY,
            System.Random random,
            Transform parent,
            PlatformGraph graph,
            List<Rect> occupiedAreas,
            float minimumSpacing,
            MovementReachProfile reachProfile,
            int maximumAttempts
        )
    {
        for (
            int attempt = 0;
            attempt < maximumAttempts;
            attempt++
        )
        {
            Vector2 candidate =
                GenerateCandidatePosition(
                    previousPlatform.Position,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    reachProfile
                );

            Rect candidateBounds =
                CreateRect(
                    candidate
                );

            if (OverlapsAny(
                    candidateBounds,
                    occupiedAreas,
                    minimumSpacing))
            {
                continue;
            }

            GeneratedPlatform
                candidatePlatform =
                new GeneratedPlatform(
                    candidate,
                    candidateBounds,
                    index,
                    false,
                    isExit,
                    routeType
                );

            ReachabilityCheckResult
                reachability =
                reachabilityValidator.Check(
                    previousPlatform,
                    candidatePlatform,
                    reachProfile
                );

            if (!reachability.IsReachable)
            {
                continue;
            }

            return CreatePlatform(
                candidate,
                index,
                false,
                isExit,
                routeType,
                parent,
                graph,
                occupiedAreas
            );
        }

        return null;
    }

    private Vector2 GenerateCandidatePosition(
        Vector2 previousPosition,
        float minX,
        float maxX,
        float minY,
        float maxY,
        System.Random random,
        MovementReachProfile reachProfile
    )
    {
        float horizontalDistance =
            reachProfile
                .EffectiveHorizontalDistance;

        float upwardDistance =
            reachProfile
                .EffectiveVerticalUpDistance;

        float downwardDistance =
            reachProfile
                .EffectiveVerticalDownDistance;

        float horizontalOffset =
            Mathf.Lerp(
                -horizontalDistance,
                horizontalDistance,
                (float)random.NextDouble()
            );

        float verticalOffset;

        if (random.NextDouble() < 0.5)
        {
            verticalOffset =
                Mathf.Lerp(
                    -downwardDistance,
                    0f,
                    (float)random.NextDouble()
                );
        }
        else
        {
            verticalOffset =
                Mathf.Lerp(
                    0f,
                    upwardDistance,
                    (float)random.NextDouble()
                );
        }

        float x =
            previousPosition.x +
            horizontalOffset;

        float y =
            previousPosition.y +
            verticalOffset;

        x =
            Mathf.Clamp(
                x,
                minX,
                maxX
            );

        y =
            Mathf.Clamp(
                y,
                minY,
                maxY
            );

        return new Vector2(
            x,
            y
        );
    }

    private GeneratedPlatform CreatePlatform(
        Vector2 position,
        int index,
        bool isStart,
        bool isExit,
        PlatformRouteType routeType,
        Transform parent,
        PlatformGraph graph,
        List<Rect> occupiedAreas
    )
    {
        Rect bounds =
            CreateRect(
                position
            );

        UnityEngine.Object.Instantiate(
            platformPrefab,
            position,
            Quaternion.identity,
            parent
        );

        GeneratedPlatform platform =
            new GeneratedPlatform(
                position,
                bounds,
                index,
                isStart,
                isExit,
                routeType
            );

        graph.AddPlatform(
            platform
        );

        occupiedAreas.Add(
            bounds
        );

        return platform;
    }

    private Rect CreateRect(
        Vector2 center
    )
    {
        return new Rect(
            center.x -
                platformSize.x / 2f,

            center.y -
                platformSize.y / 2f,

            platformSize.x,
            platformSize.y
        );
    }

    private bool OverlapsAny(
        Rect candidate,
        List<Rect> occupiedAreas,
        float minimumSpacing
    )
    {
        foreach (
            Rect occupiedArea
            in occupiedAreas)
        {
            Rect expandedArea =
                ExpandRect(
                    occupiedArea,
                    minimumSpacing
                );

            if (candidate.Overlaps(
                    expandedArea))
            {
                return true;
            }
        }

        return false;
    }

    private Rect ExpandRect(
        Rect rect,
        float amount
    )
    {
        return new Rect(
            rect.xMin -
                amount / 2f,

            rect.yMin -
                amount / 2f,

            rect.width +
                amount,

            rect.height +
                amount
        );
    }

    private void CalculateGenerationLimits(
        RoomGenerationBounds bounds,
        out float minX,
        out float maxX,
        out float minY,
        out float maxY
    )
    {
        Vector2 center =
            bounds.Center;

        Vector2 size =
            bounds.Size;

        float halfWidth =
            platformSize.x / 2f;

        float halfHeight =
            platformSize.y / 2f;

        minX =
            center.x -
            size.x / 2f +
            halfWidth;

        maxX =
            center.x +
            size.x / 2f -
            halfWidth;

        minY =
            center.y -
            size.y / 2f +
            halfHeight;

        maxY =
            center.y +
            size.y / 2f -
            halfHeight;
    }

    private bool ValidateInput(
        RoomGenerationBounds bounds,
        System.Random random,
        Transform parent,
        int count,
        int maximumAttempts,
        MovementReachProfile reachProfile
    )
    {
        if (bounds == null)
            return false;

        if (random == null)
            return false;

        if (parent == null)
            return false;

        if (count <= 0)
            return false;

        if (maximumAttempts <= 0)
            return false;

        if (reachProfile == null)
            return false;

        return true;
    }

    private bool HasValidPlatformSize()
    {
        if (platformSize.x > 0f &&
            platformSize.y > 0f)
        {
            return true;
        }

        Debug.LogError(
            "PlatformGenerator: " +
            "Platform prefab has no valid " +
            "ProceduralPlatform size."
        );

        return false;
    }

    private PlatformGenerationResult
        CreateResult(
            PlatformGraph graph,
            GeneratedPlatform startPlatform,
            GeneratedPlatform exitPlatform,
            List<GeneratedPlatform> mainPathPlatforms,
            List<GeneratedPlatform> branchPlatforms,
            List<GeneratedPlatform> deadEndPlatforms
        )
    {
        return new PlatformGenerationResult(
            graph,
            startPlatform,
            exitPlatform,
            mainPathPlatforms,
            branchPlatforms,
            deadEndPlatforms
        );
    }
}