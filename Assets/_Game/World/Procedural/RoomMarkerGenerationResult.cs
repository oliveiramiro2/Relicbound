public class RoomMarkerGenerationResult
{
  public GeneratedRoomMarker
      PlayerSpawn
  { get; }

  public GeneratedRoomMarker
      RoomEntry
  { get; }

  public GeneratedRoomMarker
      RoomExit
  { get; }

  public RoomMarkerGenerationResult(
      GeneratedRoomMarker playerSpawn,
      GeneratedRoomMarker roomEntry,
      GeneratedRoomMarker roomExit
  )
  {
    PlayerSpawn = playerSpawn;
    RoomEntry = roomEntry;
    RoomExit = roomExit;
  }
}