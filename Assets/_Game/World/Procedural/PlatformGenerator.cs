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

        if (platformPrefab != null)
        {
            ProceduralPlatform platform =
                platformPrefab.GetComponent<ProceduralPlatform>();

            if (platform != null)
            {
                platformSize =
                    platform.Size;
            }
        }
    }

    public void Generate(
        RoomGenerationBounds bounds,
        System.Random random,
        Transform parent,
        int count
    )
    {
        if (bounds == null)
            return;

        if (platformPrefab == null)
            return;

        if (random == null)
            return;

        if (count <= 0)
            return;

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
            return;
        }

        List<Rect> occupiedAreas =
            new List<Rect>();

        int generatedCount = 0;

        int maxAttempts =
            count * 20;

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
                    occupiedAreas))
            {
                continue;
            }

            UnityEngine.Object.Instantiate(
                platformPrefab,
                position,
                Quaternion.identity,
                parent
            );

            occupiedAreas.Add(candidate);

            generatedCount++;
        }

        Debug.Log(
            $"Generated {generatedCount}/{count} platforms."
        );
    }

    private Rect CreateRect(
        Vector2 center
    )
    {
        return new Rect(
            center.x - platformSize.x / 2f,
            center.y - platformSize.y / 2f,
            platformSize.x,
            platformSize.y
        );
    }

    private bool OverlapsAny(
        Rect candidate,
        List<Rect> occupiedAreas
    )
    {
        foreach (Rect occupiedArea in occupiedAreas)
        {
            if (candidate.Overlaps(
                    occupiedArea))
            {
                return true;
            }
        }

        return false;
    }
}