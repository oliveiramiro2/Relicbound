using UnityEngine;

public class RoomMarkerGenerator
{
    private readonly GameObject playerSpawnPrefab;

    private readonly GameObject roomEntryPrefab;

    private readonly float
        playerSpawnVerticalOffset;

    private readonly float
        roomEntryVerticalOffset;

    public RoomMarkerGenerator(
        GameObject playerSpawnPrefab,
        GameObject roomEntryPrefab,
        float playerSpawnVerticalOffset,
        float roomEntryVerticalOffset
    )
    {
        this.playerSpawnPrefab =
            playerSpawnPrefab;

        this.roomEntryPrefab =
            roomEntryPrefab;

        this.playerSpawnVerticalOffset =
            Mathf.Max(
                0f,
                playerSpawnVerticalOffset
            );

        this.roomEntryVerticalOffset =
            Mathf.Max(
                0f,
                roomEntryVerticalOffset
            );
    }

    public RoomMarkerGenerationResult Generate(
        PlatformGenerationResult result,
        Transform parent
    )
    {
        if (!ValidateInput(
                result,
                parent))
        {
            return new RoomMarkerGenerationResult(
                null,
                null,
                null
            );
        }

        GeneratedRoomMarker playerSpawn =
            CreateMarker(
                result.StartPlatform,
                parent,
                playerSpawnPrefab,
                playerSpawnVerticalOffset,
                ProceduralRoomMarker.MarkerType
                    .PlayerSpawn
            );

        GeneratedRoomMarker roomEntry =
            CreateMarker(
                result.StartPlatform,
                parent,
                roomEntryPrefab,
                roomEntryVerticalOffset,
                ProceduralRoomMarker.MarkerType
                    .RoomEntry
            );

        return new RoomMarkerGenerationResult(
            playerSpawn,
            roomEntry,
            result.ExitPlatform
        );
    }

    private GeneratedRoomMarker CreateMarker(
        GeneratedPlatform platform,
        Transform parent,
        GameObject prefab,
        float verticalOffset,
        ProceduralRoomMarker.MarkerType markerType
    )
    {
        if (platform == null)
            return null;

        Vector2 position =
            new Vector2(
                platform.Position.x,
                platform.Bounds.yMax +
                verticalOffset
            );

        if (prefab != null)
        {
            Object.Instantiate(
                prefab,
                position,
                Quaternion.identity,
                parent
            );
        }

        return new GeneratedRoomMarker(
            platform,
            position,
            markerType
        );
    }

    private bool ValidateInput(
        PlatformGenerationResult result,
        Transform parent
    )
    {
        if (result == null)
            return false;

        if (parent == null)
            return false;

        if (result.StartPlatform == null)
            return false;

        if (result.ExitPlatform == null)
            return false;

        return true;
    }
}