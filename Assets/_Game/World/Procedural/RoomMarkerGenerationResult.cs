using System.Collections.Generic;

public class RoomMarkerGenerationResult
{
  public GeneratedRoomMarker
      PlayerSpawn
  { get; }

  public GeneratedRoomMarker
      RoomExit
  { get; }

  public RoomMarkerGenerationResult(
      GeneratedRoomMarker playerSpawn,
      GeneratedRoomMarker roomExit
  )
  {
    PlayerSpawn = playerSpawn;
    RoomExit = roomExit;
  }
}