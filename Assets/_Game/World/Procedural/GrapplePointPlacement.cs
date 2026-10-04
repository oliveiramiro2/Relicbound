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

        Vector2 position =
            GetSurfacePosition(
                platform,
                direction
            );

        position +=
            direction *
            distanceFromPlatform;

        return position;
    }

    private static Vector2 GetSurfacePosition(
        GeneratedPlatform platform,
        Vector2 direction
    )
    {
        Rect bounds =
            platform.Bounds;

        float horizontal =
            Mathf.Abs(direction.x);

        float vertical =
            Mathf.Abs(direction.y);

        /*
         * Predominantemente horizontal:
         * usa uma das laterais da plataforma.
         */
        if (horizontal > vertical)
        {
            float x =
                direction.x >= 0f
                    ? bounds.xMax
                    : bounds.xMin;

            /*
             * Não deixa o grapple point exatamente
             * no centro vertical da plataforma.
             *
             * Isso ajuda a criar situações mais
             * naturais para o swing.
             */
            float verticalOffset =
                direction.y *
                bounds.y *
                0.35f;

            float y =
                Mathf.Clamp(
                    platform.Position.y +
                    verticalOffset,
                    bounds.yMin,
                    bounds.yMax
                );

            return new Vector2(
                x,
                y
            );
        }

        /*
         * Predominantemente vertical:
         * usa topo ou base.
         */
        float yPosition =
            direction.y >= 0f
                ? bounds.yMax
                : bounds.yMin;

        float horizontalOffset =
            direction.x *
            bounds.x *
            0.35f;

        float xPosition =
            Mathf.Clamp(
                platform.Position.x +
                horizontalOffset,
                bounds.xMin,
                bounds.xMax
            );

        return new Vector2(
            xPosition,
            yPosition
        );
    }
}