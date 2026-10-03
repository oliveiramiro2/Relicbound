using UnityEngine;

public class RoomConnectionTriggerGenerator
{
    private readonly Vector2 triggerSize;

    public RoomConnectionTriggerGenerator(
        Vector2 triggerSize
    )
    {
        this.triggerSize =
            new Vector2(
                Mathf.Max(
                    0.1f,
                    triggerSize.x
                ),
                Mathf.Max(
                    0.1f,
                    triggerSize.y
                )
            );
    }

    public void Generate(
        RoomConnectionResult result,
        Transform parent,
        RoomTransitionController
            transitionController
    )
    {
        if (result == null)
            return;

        if (parent == null)
            return;

        if (transitionController == null)
            return;

        foreach (
            RoomConnection connection
            in result.Connections)
        {
            if (connection == null)
                continue;

            CreateTrigger(
                connection,
                parent,
                transitionController
            );
        }
    }

    private void CreateTrigger(
        RoomConnection connection,
        Transform parent,
        RoomTransitionController
            transitionController
    )
    {
        GameObject triggerObject =
            new GameObject(
                "RoomTransitionTrigger"
            );

        triggerObject.transform.SetParent(
            parent
        );

        triggerObject.transform.position =
            connection.ExitPosition;

        BoxCollider2D collider =
            triggerObject.AddComponent<
                BoxCollider2D>();

        collider.isTrigger =
            true;

        collider.size =
            triggerSize;

        RoomTransitionTrigger trigger =
            triggerObject.AddComponent<
                RoomTransitionTrigger>();

        trigger.Initialize(
            connection,
            transitionController
        );
    }
}