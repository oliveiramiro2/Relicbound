using UnityEngine;

public static class RoomEntryPositionCalculator
{
  public static Vector2 Calculate(
      GeneratedRoom targetRoom,
      Vector2 fromDirection
  )
  {
    if (targetRoom == null)
      return Vector2.zero;

    if (targetRoom.Markers == null)
      return Vector2.zero;

    GeneratedPlatform platform =
        targetRoom.Markers.StartPlatform;

    if (platform == null)
      return targetRoom.Markers.RoomEntry != null
          ? targetRoom.Markers.RoomEntry.Position
          : Vector2.zero;

    if (fromDirection.sqrMagnitude <= 0.001f)
      fromDirection =
          Vector2.left;

    fromDirection.Normalize();

    float horizontal =
        Mathf.Abs(
            fromDirection.x
        );

    float vertical =
        Mathf.Abs(
            fromDirection.y
        );

    Vector2 position =
        platform.Position;

    const float offset = 1f;

    if (horizontal >= vertical)
    {
      float x;

      if (fromDirection.x >= 0f)
      {
        x =
            platform.Bounds.xMax +
            offset;
      }
      else
      {
        x =
            platform.Bounds.xMin -
            offset;
      }

      position =
          new Vector2(
              x,
              platform.Position.y
          );
    }
    else
    {
      float y;

      if (fromDirection.y >= 0f)
      {
        y =
            platform.Bounds.yMax +
            offset;
      }
      else
      {
        y =
            platform.Bounds.yMin -
            offset;
      }

      position =
          new Vector2(
              platform.Position.x,
              y
          );
    }

    return position;
  }
}