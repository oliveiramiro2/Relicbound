using UnityEngine;

public class GeneratedHazard
{
  public GeneratedPlatform Platform { get; }

  public Vector2 Position { get; }

  public Rect Bounds { get; }

  public GeneratedHazard(
      GeneratedPlatform platform,
      Vector2 position,
      Rect bounds
  )
  {
    Platform =
        platform;

    Position =
        position;

    Bounds =
        bounds;
  }
}