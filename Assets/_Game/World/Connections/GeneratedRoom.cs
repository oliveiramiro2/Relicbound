using UnityEngine;

public class GeneratedRoom
{
  public WorldRoom Room { get; }

  public GameObject Instance { get; }

  public RoomMarkerGenerationResult Markers { get; }

  public GeneratedRoom(
      WorldRoom room,
      GameObject instance,
      RoomMarkerGenerationResult markers
  )
  {
    Room = room;
    Instance = instance;
    Markers = markers;
  }
}