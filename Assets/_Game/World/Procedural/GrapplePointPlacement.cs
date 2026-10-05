using UnityEngine;

public static class GrapplePointPlacement
{
    public static Vector2 Calculate(
        GeneratedPlatform from,
        GeneratedPlatform to,
        float minimumDistanceFromPlatform,
        float preferredDistanceFromPlatform,
        float maximumDistanceFromPlatform,
        float midpointInfluence,
        float verticalOffset
    )
    {
        if (from == null)
            return Vector2.zero;

        if (to == null)
            return from.Position;

        Vector2 fromPosition =
            from.Position;

        Vector2 toPosition =
            to.Position;

        Vector2 direction =
            toPosition -
            fromPosition;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return fromPosition;
        }

        direction.Normalize();

        Vector2 perpendicular =
            new Vector2(
                -direction.y,
                direction.x
            );

        /*
         * O centro entre as plataformas é a base.
         */
        Vector2 midpoint =
            Vector2.Lerp(
                fromPosition,
                toPosition,
                0.5f
            );

        /*
         * Coloca o gancho um pouco acima
         * do eixo central.
         *
         * Isso cria uma posição de "âncora"
         * mais natural para o swing.
         */
        Vector2 candidate =
            midpoint;

        candidate +=
            Vector2.up *
            verticalOffset;

        /*
         * Pequena influência perpendicular
         * evita que todos os grapples fiquem
         * exatamente alinhados.
         */
        float perpendicularOffset =
            Mathf.Clamp(
                direction.y * 0.5f,
                -1f,
                1f
            );

        candidate +=
            perpendicular *
            perpendicularOffset;

        /*
         * Calcula a distância até cada plataforma.
         */
        float distanceFromFrom =
            DistanceFromPlatform(
                candidate,
                from
            );

        float distanceFromTo =
            DistanceFromPlatform(
                candidate,
                to
            );

        /*
         * Se o ponto ficou muito perto de uma
         * plataforma, empurra para longe dela.
         */
        if (distanceFromFrom <
            minimumDistanceFromPlatform)
        {
            candidate =
                PushAwayFromPlatform(
                    candidate,
                    from,
                    minimumDistanceFromPlatform
                );
        }

        if (distanceFromTo <
            minimumDistanceFromPlatform)
        {
            candidate =
                PushAwayFromPlatform(
                    candidate,
                    to,
                    minimumDistanceFromPlatform
                );
        }

        /*
         * Faz uma pequena correção em direção
         * ao ponto ideal entre as plataformas.
         */
        Vector2 preferred =
            midpoint +
            Vector2.up *
            verticalOffset;

        candidate =
            Vector2.Lerp(
                candidate,
                preferred,
                midpointInfluence
            );

        /*
         * Segunda verificação depois da interpolação.
         */
        candidate =
            PushAwayIfNecessary(
                candidate,
                from,
                minimumDistanceFromPlatform
            );

        candidate =
            PushAwayIfNecessary(
                candidate,
                to,
                minimumDistanceFromPlatform
            );

        /*
         * Mantém o ponto dentro de uma distância
         * razoável das plataformas.
         */
        candidate =
            LimitDistanceFromPlatforms(
                candidate,
                from,
                to,
                maximumDistanceFromPlatform
            );

        return candidate;
    }

    public static bool IsValid(
        Vector2 position,
        GeneratedPlatform from,
        GeneratedPlatform to,
        float minimumDistanceFromPlatform,
        float maximumDistanceFromPlatform
    )
    {
        if (from == null || to == null)
            return false;

        float fromDistance =
            DistanceFromPlatform(
                position,
                from
            );

        float toDistance =
            DistanceFromPlatform(
                position,
                to
            );

        if (fromDistance <
            minimumDistanceFromPlatform)
        {
            return false;
        }

        if (toDistance <
            minimumDistanceFromPlatform)
        {
            return false;
        }

        if (fromDistance >
            maximumDistanceFromPlatform)
        {
            return false;
        }

        if (toDistance >
            maximumDistanceFromPlatform)
        {
            return false;
        }

        return true;
    }

    private static Vector2 PushAwayIfNecessary(
        Vector2 position,
        GeneratedPlatform platform,
        float minimumDistance
    )
    {
        float distance =
            DistanceFromPlatform(
                position,
                platform
            );

        if (distance >= minimumDistance)
            return position;

        return PushAwayFromPlatform(
            position,
            platform,
            minimumDistance
        );
    }

    private static Vector2 PushAwayFromPlatform(
        Vector2 position,
        GeneratedPlatform platform,
        float minimumDistance
    )
    {
        Vector2 closest =
            ClosestPoint(
                position,
                platform.Bounds
            );

        Vector2 direction =
            position -
            closest;

        if (direction.sqrMagnitude <= 0.001f)
        {
            direction =
                Vector2.up;
        }

        direction.Normalize();

        return closest +
               direction *
               minimumDistance;
    }

    private static float DistanceFromPlatform(
        Vector2 position,
        GeneratedPlatform platform
    )
    {
        Vector2 closest =
            ClosestPoint(
                position,
                platform.Bounds
            );

        return Vector2.Distance(
            position,
            closest
        );
    }

    private static Vector2 ClosestPoint(
        Vector2 position,
        Rect bounds
    )
    {
        return new Vector2(
            Mathf.Clamp(
                position.x,
                bounds.xMin,
                bounds.xMax
            ),
            Mathf.Clamp(
                position.y,
                bounds.yMin,
                bounds.yMax
            )
        );
    }

    private static Vector2 LimitDistanceFromPlatforms(
        Vector2 position,
        GeneratedPlatform from,
        GeneratedPlatform to,
        float maximumDistance
    )
    {
        Vector2 midpoint =
            Vector2.Lerp(
                from.Position,
                to.Position,
                0.5f
            );

        Vector2 direction =
            position -
            midpoint;

        float distance =
            direction.magnitude;

        if (distance <= maximumDistance)
            return position;

        if (distance <= 0.001f)
            return midpoint;

        direction.Normalize();

        return midpoint +
               direction *
               maximumDistance;
    }
}