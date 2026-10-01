using UnityEngine;

public class GeneratedPlatform
{
  public Vector2 Position { get; }

  public Rect Bounds { get; }

  public int Index { get; }

  public bool IsStart { get; }

  public bool IsExit { get; }

  public GeneratedPlatform(
      Vector2 position,
      Rect bounds,
      int index,
      bool isStart,
      bool isExit
  )
  {
    Position = position;
    Bounds = bounds;
    Index = index;
    IsStart = isStart;
    IsExit = isExit;
  }
}