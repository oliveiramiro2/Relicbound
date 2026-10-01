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

    public int Generate(
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
        if (!IsValidInput(
                bounds,
                random,
                parent,
                count,
                maximumAttemptsPerPlatform))
        {
            return 0;
        }

        if (!HasValidPlatformSize())
            return 0;

        Vector2 center =
            bounds.Center;

        Vector2 boundsSize =
            bounds.Size;

        float halfWidth =
            platformSize.x / 2f;

        float halfHeight =
            platformSize.y / 2f;

        float minX =
            center.x -
            boundsSize.x / 2f +
            halfWidth;

        float maxX =
            center.x +
            boundsSize.x / 2f -
            halfWidth;

        float minY =
            center.y -
            boundsSize.y / 2f +
            halfHeight;

        float maxY =
            center.y +
            boundsSize.y / 2f -
            halfHeight;

        if (minX > maxX ||
            minY > maxY)
        {
            Debug.LogWarning(
                "PlatformGenerator: " +
                "Generation bounds are too small."
            );

            return 0;
        }

        minimumSpacing =
            Mathf.Max(
                0f,
                minimumSpacing
            );

        maximumHorizontalDistance =
            Mathf.Max(
                0f,
                maximumHorizontalDistance
            );

        maximumVerticalDistance =
            Mathf.Max(
                0f,
                maximumVerticalDistance
            );

        List<Rect> occupiedAreas =
            new List<Rect>();

        List<Vector2> generatedPositions =
            new List<Vector2>();

        int generatedCount = 0;

        Vector2 firstPosition =
            GenerateFirstPosition(
                minX,
                maxX,
                minY,
                maxY,
                random
            );

        CreatePlatform(
            firstPosition,
            parent,
            occupiedAreas,
            generatedPositions
        );

        generatedCount++;

        for (
            int platformIndex = 1;
            platformIndex < count;
            platformIndex++
        )
        {
            bool generated =
                TryGenerateNextPlatform(
                    generatedPositions[
                        generatedPositions.Count - 1
                    ],
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    occupiedAreas,
                    generatedPositions,
                    parent,
                    minimumSpacing,
                    maximumHorizontalDistance,
                    minimumVerticalDistance,
                    maximumVerticalDistance,
                    maximumAttemptsPerPlatform
                );

            if (!generated)
            {
                Debug.LogWarning(
                    $"PlatformGenerator: " +
                    $"Could not generate platform " +
                    $"{platformIndex + 1}/{count}."
                );

                break;
            }

            generatedCount++;
        }

        Debug.Log(
            $"PlatformGenerator: " +
            $"Generated {generatedCount}/{count} platforms."
        );

        return generatedCount;
    }

    private bool TryGenerateNextPlatform(
        Vector2 previousPosition,
        float minX,
        float maxX,
        float minY,
        float maxY,
        System.Random random,
        List<Rect> occupiedAreas,
        List<Vector2> generatedPositions,
        Transform parent,
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
                    previousPosition,
                    minX,
                    maxX,
                    minY,
                    maxY,
                    random,
                    maximumHorizontalDistance,
                    minimumVerticalDistance,
                    maximumVerticalDistance
                );

            Rect candidateRect =
                CreateRect(candidate);

            if (OverlapsAny(
                    candidateRect,
                    occupiedAreas,
                    minimumSpacing))
            {
                continue;
            }

            CreatePlatform(
                candidate,
                parent,
                occupiedAreas,
                generatedPositions
            );

            return true;
        }

        return false;
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

    private Vector2 GenerateFirstPosition(
        float minX,
        float maxX,
        float minY,
        float maxY,
        System.Random random
    )
    {
        float x =
            Mathf.Lerp(
                minX,
                maxX,
                (float)random.NextDouble()
            );

        float y =
            Mathf.Lerp(
                minY,
                maxY,
                (float)random.NextDouble()
            );

        return new Vector2(
            x,
            y
        );
    }

    private void CreatePlatform(
        Vector2 position,
        Transform parent,
        List<Rect> occupiedAreas,
        List<Vector2> generatedPositions
    )
    {
        UnityEngine.Object.Instantiate(
            platformPrefab,
            position,
            Quaternion.identity,
            parent
        );

        occupiedAreas.Add(
            CreateRect(position)
        );

        generatedPositions.Add(
            position
        );
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

    private bool IsValidInput(
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