using UnityEngine;

public class GeneratedRoomMarker
{
  public GeneratedPlatform Platform { get; }

  public Vector2 Position { get; }

  public ProceduralRoomMarker.MarkerType Type { get; }

  public GeneratedRoomMarker(
      GeneratedPlatform platform,
      Vector2 position,
      ProceduralRoomMarker.MarkerType type
  )
  {
    Platform = platform;
    Position = position;
    Type = type;
  }
}