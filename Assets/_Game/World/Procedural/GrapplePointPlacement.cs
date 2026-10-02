using UnityEngine;

public static class GrapplePointPlacement
{
  public static Vector2 Calculate(
      GeneratedPlatform platform,
      GeneratedPlatform target,
      float distanceFromPlatform
  )
  {
    if (platform == null)
      return Vector2.zero;

    if (target == null)
      return platform.Position;

    Vector2 direction =
        target.Position -
        platform.Position;

    if (direction.sqrMagnitude <= 0.001f)
    {
      return new Vector2(
          platform.Position.x,
          platform.Bounds.yMax +
          distanceFromPlatform
      );
    }

    direction.Normalize();

    float horizontalDirection =
        Mathf.Abs(direction.x);

    float verticalDirection =
        Mathf.Abs(direction.y);

    Vector2 pointPosition =
        platform.Position;

    if (horizontalDirection >=
        verticalDirection)
    {
      float x =
          direction.x >= 0f
              ? platform.Bounds.xMax
              : platform.Bounds.xMin;

      pointPosition =
          new Vector2(
              x,
              platform.Position.y
          );
    }
    else
    {
      float y =
          direction.y >= 0f
              ? platform.Bounds.yMax
              : platform.Bounds.yMin;

      pointPosition =
          new Vector2(
              platform.Position.x,
              y
          );
    }

    pointPosition +=
        direction *
        distanceFromPlatform;

    return pointPosition;
  }
}