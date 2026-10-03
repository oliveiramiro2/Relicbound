using UnityEngine;

public class RoomTransitionController :
    MonoBehaviour
{
  [SerializeField]
  private Transform player;

  private GeneratedWorld generatedWorld;

  private WorldRoom currentRoom;

  private bool isTransitioning;

  public GeneratedWorld GeneratedWorld =>
      generatedWorld;

  public WorldRoom CurrentRoom =>
      currentRoom;

  public bool IsTransitioning =>
      isTransitioning;

  public void Initialize(
      GeneratedWorld world
  )
  {
    generatedWorld =
        world;

    currentRoom =
        FindStartRoom(
            world
        );

    isTransitioning =
        false;
  }

  private void Update()
  {
    if (currentRoom == null)
      return;

    Debug.Log(
        "Current Room: " +
        currentRoom.Id +
        " | Type: " +
        currentRoom.Type
    );
  }

  public void Transition(
    RoomConnection connection
)
  {
    if (isTransitioning)
      return;

    if (connection == null)
      return;

    if (generatedWorld == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "GeneratedWorld has not been initialized."
      );

      return;
    }

    if (currentRoom == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "CurrentRoom is null."
      );

      return;
    }

    if (connection.From != currentRoom)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "Connection does not start " +
          "from the current room."
      );

      return;
    }

    GeneratedRoom targetRoom =
        generatedWorld.GetRoom(
            connection.To
        );

    if (targetRoom == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "Target room was not found."
      );

      return;
    }

    isTransitioning =
        true;

    MovePlayer(
        connection.EntryPosition
    );

    currentRoom =
        connection.To;

    isTransitioning =
        false;
  }

  private void MovePlayer(
      Vector2 position
  )
  {
    if (player == null)
    {
      Debug.LogWarning(
          "RoomTransitionController: " +
          "Player reference is missing."
      );

      return;
    }

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

  private WorldRoom FindStartRoom(
      GeneratedWorld world
  )
  {
    if (world == null)
      return null;

    foreach (
        GeneratedRoom generatedRoom
        in world.Rooms)
    {
      if (generatedRoom == null)
        continue;

      if (generatedRoom.Room == null)
        continue;

      if (generatedRoom.Room.Type ==
          WorldRoomType.Start)
      {
        return generatedRoom.Room;
      }
    }

    return null;
  }
}