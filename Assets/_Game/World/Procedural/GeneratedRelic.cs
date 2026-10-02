using UnityEngine;

public class GeneratedRelic
{
  public GeneratedPlatform Platform { get; }

  public RelicData Data { get; }

  public Vector2 Position { get; }

  public GeneratedRelic(
      GeneratedPlatform platform,
      RelicData data,
      Vector2 position
  )
  {
    Platform = platform;
    Data = data;
    Position = position;
  }
}