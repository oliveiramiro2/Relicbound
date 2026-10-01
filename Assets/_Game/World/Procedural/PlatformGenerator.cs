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

        platformSize = Vector2.zero;

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
        float minimumSpacing
    )
    {
        if (bounds == null)
            return 0;

        if (platformPrefab == null)
            return 0;

        if (random == null)
            return 0;

        if (parent == null)
            return 0;

        if (count <= 0)
            return 0;

        if (platformSize.x <= 0f ||
            platformSize.y <= 0f)
        {
            Debug.LogError(
                "PlatformGenerator: " +
                "Platform prefab has an invalid size."
            );

            return 0;
        }

        minimumSpacing =
            Mathf.Max(
                0f,
                minimumSpacing
            );

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
                "Generation bounds are too small " +
                "for the platform."
            );

            return 0;
        }

        List<Rect> occupiedAreas =
            new List<Rect>();

        int generatedCount = 0;

        int maxAttempts =
            Mathf.Max(
                count * 30,
                30
            );

        for (
            int attempt = 0;
            attempt < maxAttempts &&
            generatedCount < count;
            attempt++)
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

            Vector2 position =
                new Vector2(
                    x,
                    y
                );

            Rect candidate =
                CreateRect(position);

            if (OverlapsAny(
                    candidate,
                    occupiedAreas,
                    minimumSpacing))
            {
                continue;
            }

            UnityEngine.Object.Instantiate(
                platformPrefab,
                position,
                Quaternion.identity,
                parent
            );

            occupiedAreas.Add(
                candidate
            );

            generatedCount++;
        }

        if (generatedCount < count)
        {
            Debug.LogWarning(
                $"PlatformGenerator: " +
                $"Generated {generatedCount}/{count} " +
                $"platforms."
            );
        }

        return generatedCount;
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
}