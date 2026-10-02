using UnityEngine;

public class RoomMarkerGenerator
{
  private readonly GameObject playerSpawnPrefab;
  private readonly GameObject roomExitPrefab;

  private readonly float
      playerSpawnVerticalOffset;

  private readonly float
      roomExitVerticalOffset;

  public RoomMarkerGenerator(
      GameObject playerSpawnPrefab,
      GameObject roomExitPrefab,
      float playerSpawnVerticalOffset,
      float roomExitVerticalOffset
  )
  {
    this.playerSpawnPrefab =
        playerSpawnPrefab;

    this.roomExitPrefab =
        roomExitPrefab;

    this.playerSpawnVerticalOffset =
        Mathf.Max(
            0f,
            playerSpawnVerticalOffset
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
          null
      );
    }

    GeneratedRoomMarker playerSpawn =
        CreatePlayerSpawn(
            result.StartPlatform,
            parent
        );

    GeneratedRoomMarker roomExit =
        CreateRoomExit(
            result.ExitPlatform,
            parent
        );

    return new RoomMarkerGenerationResult(
        playerSpawn,
        roomExit
    );
  }

  private GeneratedRoomMarker
      CreatePlayerSpawn(
          GeneratedPlatform platform,
          Transform parent
      )
  {
    if (platform == null)
      return null;

    Vector2 position =
        CalculatePosition(
            platform,
            playerSpawnVerticalOffset
        );

    if (playerSpawnPrefab != null)
    {
      UnityEngine.Object.Instantiate(
          playerSpawnPrefab,
          position,
          Quaternion.identity,
          parent
      );
    }

    return new GeneratedRoomMarker(
        platform,
        position,
        ProceduralRoomMarker.MarkerType
            .PlayerSpawn
    );
  }

  private GeneratedRoomMarker
      CreateRoomExit(
          GeneratedPlatform platform,
          Transform parent
      )
  {
    if (platform == null)
      return null;

    Vector2 position =
        CalculatePosition(
            platform,
            roomExitVerticalOffset
        );

    if (roomExitPrefab != null)
    {
      UnityEngine.Object.Instantiate(
          roomExitPrefab,
          position,
          Quaternion.identity,
          parent
      );
    }

    return new GeneratedRoomMarker(
        platform,
        position,
        ProceduralRoomMarker.MarkerType
            .RoomExit
    );
  }

  private Vector2 CalculatePosition(
      GeneratedPlatform platform,
      float verticalOffset
  )
  {
    return new Vector2(
        platform.Position.x,
        platform.Bounds.yMax +
        verticalOffset
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