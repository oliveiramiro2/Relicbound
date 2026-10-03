using UnityEngine;

[RequireComponent(
    typeof(Collider2D)
)]
public class RoomTransitionTrigger :
    MonoBehaviour
{
  private RoomConnection connection;

  private RoomTransitionController
      transitionController;

  public void Initialize(
      RoomConnection roomConnection,
      RoomTransitionController controller
  )
  {
    connection =
        roomConnection;

    transitionController =
        controller;
  }

  private void Awake()
  {
    Collider2D collider =
        GetComponent<Collider2D>();

    collider.isTrigger =
        true;
  }

  private void OnTriggerEnter2D(
      Collider2D other
  )
  {
    if (connection == null)
      return;

    if (transitionController == null)
      return;

    PlayerMovement player =
        other.GetComponent<
            PlayerMovement>();

    if (player == null)
      return;

    transitionController.Transition(
        connection
    );
  }
}