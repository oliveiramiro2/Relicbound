using UnityEngine;

public class RoomMarkerGenerator
{
    private readonly GameObject playerSpawnPrefab;
    private readonly GameObject roomEntryPrefab;
    private readonly GameObject roomExitPrefab;

    private readonly float
        playerSpawnVerticalOffset;

    private readonly float
        roomEntryVerticalOffset;

    private readonly float
        roomExitVerticalOffset;

    public RoomMarkerGenerator(
        GameObject playerSpawnPrefab,
        GameObject roomEntryPrefab,
        GameObject roomExitPrefab,
        float playerSpawnVerticalOffset,
        float roomEntryVerticalOffset,
        float roomExitVerticalOffset
    )
    {
        this.playerSpawnPrefab =
            playerSpawnPrefab;

        this.roomEntryPrefab =
            roomEntryPrefab;

        this.roomExitPrefab =
            roomExitPrefab;

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

        this.roomExitVerticalOffset =
            Mathf.Max(
                0f,
                roomExitVerticalOffset
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

        GeneratedRoomMarker roomExit =
            null;

        return new RoomMarkerGenerationResult(
            playerSpawn,
            roomEntry,
            roomExit
        );
    }

    private GeneratedRoomMarker CreateMarker(
        GeneratedPlatform platform,
        Transform parent,
        GameObject prefab,
        float verticalOffset,
        ProceduralRoomMarker.MarkerType type
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
            UnityEngine.Object.Instantiate(
                prefab,
                position,
                Quaternion.identity,
                parent
            );
        }

        return new GeneratedRoomMarker(
            platform,
            position,
            type
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