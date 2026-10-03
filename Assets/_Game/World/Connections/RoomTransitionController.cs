using UnityEngine;

public class RoomTransitionController :
    MonoBehaviour
{
  [SerializeField]
  private Transform player;

  private bool isTransitioning;

  public bool IsTransitioning =>
      isTransitioning;

  [SerializeField]
  private Vector2 connectionEntryOffset =
      new Vector2(
          0f,
          1f
      );

  public void Transition(
      RoomConnection connection
  )
  {
    if (isTransitioning)
      return;

    if (connection == null)
      return;

    if (player == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "Player reference is missing."
      );

      return;
    }

    GeneratedWorld world =
        FindAnyObjectByType<WorldGenerationController>()?.GeneratedWorld;

    if (world == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "GeneratedWorld not found."
      );

      return;
    }

    GeneratedRoom targetRoom =
        world.GetRoom(
            connection.To
        );

    if (targetRoom == null)
      return;

    if (targetRoom.Markers == null)
      return;

    GeneratedRoomMarker entry =
        targetRoom.Markers.RoomEntry;

    if (entry == null)
      return;

    isTransitioning = true;

    MovePlayer(
      entry.Position +
      connectionEntryOffset
    );

    isTransitioning = false;
  }

  private void MovePlayer(
      Vector2 position
  )
  {
    Rigidbody2D body =
        player.GetComponent<
            Rigidbody2D>();

    if (body != null)
    {
      body.linearVelocity =
          Vector2.zero;

      body.angularVelocity =
          0f;
    }

    player.position =
        position;
  }
}