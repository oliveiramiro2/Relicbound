using UnityEngine;

public class RoomExitGenerator
{
  private readonly GameObject exitPrefab;

  private readonly float
      horizontalOffset;

  private readonly float
      verticalOffset;

  public RoomExitGenerator(
      GameObject exitPrefab,
      float horizontalOffset,
      float verticalOffset
  )
  {
    this.exitPrefab =
        exitPrefab;

    this.horizontalOffset =
        Mathf.Max(
            0f,
            horizontalOffset
        );

    this.verticalOffset =
        Mathf.Max(
            0f,
            verticalOffset
        );
  }

  public GeneratedRoomExit Generate(
      WorldRoom from,
      WorldRoom to,
      GeneratedPlatform platform,
      Transform parent
  )
  {
    if (from == null)
      return null;

    if (to == null)
      return null;

    if (platform == null)
      return null;

    if (parent == null)
      return null;

    Vector2 direction =
        CalculateDirection(
            from,
            to
        );

    Vector2 position =
        CalculatePosition(
            platform,
            direction
        );

    if (exitPrefab != null)
    {
      GameObject exit =
          Object.Instantiate(
              exitPrefab,
              position,
              Quaternion.identity,
              parent
          );

      exit.transform.right =
          direction;
    }

    return new GeneratedRoomExit(
        from,
        to,
        position,
        direction
    );
  }

  private Vector2 CalculateDirection(
      WorldRoom from,
      WorldRoom to
  )
  {
    // Neste estágio usamos os IDs
    // apenas como fallback para uma
    // direção estável.

    if (to.Id > from.Id)
    {
      return Vector2.right;
    }

    return Vector2.left;
  }

  private Vector2 CalculatePosition(
      GeneratedPlatform platform,
      Vector2 direction
  )
  {
    float x;

    if (direction.x >= 0f)
    {
      x =
          platform.Bounds.xMax +
          horizontalOffset;
    }
    else
    {
      x =
          platform.Bounds.xMin -
          horizontalOffset;
    }

    float y =
        platform.Bounds.yMax +
        verticalOffset;

    return new Vector2(
        x,
        y
    );
  }
}