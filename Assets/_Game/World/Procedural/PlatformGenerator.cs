using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator
{
    private readonly GameObject platformPrefab;
    private readonly Vector2 platformSize;

    public PlatformGenerator(
        GameObject platformPrefab
    )
    {
        this.platformPrefab =
            platformPrefab;

        platformSize =
            Vector2.zero;

        if (platformPrefab == null)
            return;

        ProceduralPlatform platform =
            platformPrefab.GetComponent<ProceduralPlatform>();

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
        float maximumHorizontalDistance,
        float minimumVerticalDistance,
        float maximumVerticalDistance,
        int maximumAttemptsPerPlatform
    )
    {
        PlatformGraph graph =
            new PlatformGraph();

        if (!ValidateInput(
                bounds,
                random,
                parent,
                count,
                maximumAttemptsPerPlatform))
        {
            return new PlatformGenerationResult(
                graph,
                null,
                null
            );
        }

        if (!HasValidPlatformSize())
        {
            return new PlatformGenerationResult(
                graph,
                null,
                null
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
            return new PlatformGenerationResult(
                graph,
                null,
                null
            );
        }

        List<Rect> occupiedAreas =
            new List<Rect>();

        GeneratedPlatform startPlatform =
            GenerateStartPlatform(
                minX,
                maxX,
                minY,
                maxY,
                random,
                parent,
                graph,
                occupiedAreas
            );

        if (startPlatform == null)
        {
            return new PlatformGenerationResult(
                graph,
                null,
                null
            );
        }

        GeneratedPlatform previousPlatform =
            startPlatform;

        for (
            int index = 1;
            index < count;
            index++
        )
        {
            bool isExit =
                index == count - 1;

            GeneratedPlatform nextPlatform =
                TryGeneratePlatform(
                    previousPlatform,
                    index,
                    isExit,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    parent,
                    graph,
                    occupiedAreas,
                    minimumSpacing,
                    maximumHorizontalDistance,
                    minimumVerticalDistance,
                    maximumVerticalDistance,
                    maximumAttemptsPerPlatform
                );

            if (nextPlatform == null)
            {
                break;
            }

            graph.Connect(
                previousPlatform,
                nextPlatform
            );

            previousPlatform =
                nextPlatform;
        }

        GeneratedPlatform exitPlatform =
            previousPlatform.IsExit
                ? previousPlatform
                : null;

        return new PlatformGenerationResult(
            graph,
            startPlatform,
            exitPlatform
        );
    }

    private GeneratedPlatform GenerateStartPlatform(
        float minX,
        float maxX,
        float minY,
        float maxY,
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

        float y =
            minY;

        Vector2 position =
            new Vector2(
                x,
                y
            );

        return CreatePlatform(
            position,
            0,
            true,
            false,
            parent,
            graph,
            occupiedAreas
        );
    }

    private GeneratedPlatform TryGeneratePlatform(
        GeneratedPlatform previousPlatform,
        int index,
        bool isExit,
        float minX,
        float maxX,
        float minY,
        float maxY,
        System.Random random,
        Transform parent,
        PlatformGraph graph,
        List<Rect> occupiedAreas,
        float minimumSpacing,
        float maximumHorizontalDistance,
        float minimumVerticalDistance,
        float maximumVerticalDistance,
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
                    maximumHorizontalDistance,
                    minimumVerticalDistance,
                    maximumVerticalDistance
                );

            Rect candidateBounds =
                CreateRect(candidate);

            if (OverlapsAny(
                    candidateBounds,
                    occupiedAreas,
                    minimumSpacing))
            {
                continue;
            }

            return CreatePlatform(
                candidate,
                index,
                false,
                isExit,
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
        float maximumHorizontalDistance,
        float minimumVerticalDistance,
        float maximumVerticalDistance
    )
    {
        float horizontalOffset =
            Mathf.Lerp(
                -maximumHorizontalDistance,
                maximumHorizontalDistance,
                (float)random.NextDouble()
            );

        float verticalOffset =
            Mathf.Lerp(
                minimumVerticalDistance,
                maximumVerticalDistance,
                (float)random.NextDouble()
            );

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
        Transform parent,
        PlatformGraph graph,
        List<Rect> occupiedAreas
    )
    {
        Rect bounds =
            CreateRect(position);

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
                isExit
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
            rect.xMin - amount / 2f,
            rect.yMin - amount / 2f,
            rect.width + amount,
            rect.height + amount
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
        int maximumAttempts
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
}