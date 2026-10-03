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
        Transform parent,
        Vector2 direction
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

        if (direction.sqrMagnitude <= 0.001f)
        {
            direction =
                Vector2.right;
        }

        direction.Normalize();

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

            RotateExit(
                exit,
                direction
            );
        }

        return new GeneratedRoomExit(
            from,
            to,
            position,
            direction
        );
    }

    private Vector2 CalculatePosition(
        GeneratedPlatform platform,
        Vector2 direction
    )
    {
        float horizontal =
            Mathf.Abs(
                direction.x
            );

        float vertical =
            Mathf.Abs(
                direction.y
            );

        Vector2 position =
            platform.Position;

        if (horizontal >= vertical)
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

            position =
                new Vector2(
                    x,
                    platform.Position.y
                );
        }
        else
        {
            float y;

            if (direction.y >= 0f)
            {
                y =
                    platform.Bounds.yMax +
                    verticalOffset;
            }
            else
            {
                y =
                    platform.Bounds.yMin -
                    verticalOffset;
            }

            position =
                new Vector2(
                    platform.Position.x,
                    y
                );
        }

        return position;
    }

    private void RotateExit(
        GameObject exit,
        Vector2 direction
    )
    {
        if (exit == null)
            return;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) *
            Mathf.Rad2Deg;

        exit.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angle
            );
    }
}