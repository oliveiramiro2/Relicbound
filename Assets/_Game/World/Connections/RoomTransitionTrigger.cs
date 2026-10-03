using UnityEngine;

[RequireComponent(
    typeof(Collider2D)
)]
public class RoomTransitionTrigger :
    MonoBehaviour
{
  private RoomConnection connection;

  public void Initialize(
      RoomConnection roomConnection
  )
  {
    connection =
        roomConnection;
  }

  private void Awake()
  {
    Collider2D collider =
        GetComponent<Collider2D>();

    collider.isTrigger = true;
  }

  private void OnTriggerEnter2D(
      Collider2D other
  )
  {
    if (connection == null)
      return;

    PlayerMovement player =
        other.GetComponent<
            PlayerMovement>();

    if (player == null)
      return;

    RoomTransitionController controller =
        FindAnyObjectByType<
            RoomTransitionController>();

    if (controller == null)
      return;

    controller.Transition(
        connection
    );
  }
}