using System;
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

    public GameObject Generate(
        RoomGenerationBounds bounds,
        System.Random random,
        Transform parent
    )
    {
        if (bounds == null)
            return null;

        if (platformPrefab == null)
            return null;

        if (random == null)
            return null;

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
            return null;
        }

        Debug.Log(
            $"Bounds Center: {center} | " +
            $"Bounds Size: {boundsSize} | " +
            $"Platform Size: {platformSize} | " +
            $"MinX: {minX} | " +
            $"MaxX: {maxX} | " +
            $"MinY: {minY} | " +
            $"MaxY: {maxY}"
        );

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

        return UnityEngine.Object.Instantiate(
            platformPrefab,
            position,
            Quaternion.identity,
            parent
        );
    }
}