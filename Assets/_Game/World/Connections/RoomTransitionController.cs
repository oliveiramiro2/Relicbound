using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomTransitionController : MonoBehaviour
{
  [Header("Player")]
  [SerializeField] private Transform player;

  [Header("Transition")]
  [SerializeField] private RoomTransitionFader transitionFader;
  [SerializeField] private float fadeOutDuration = 0.2f;
  [SerializeField] private float fadeInDuration = 0.2f;

  private GeneratedWorld generatedWorld;
  private WorldRoom currentRoom;

  private RoomTransitionState state =
      RoomTransitionState.Idle;

  public GeneratedWorld GeneratedWorld =>
      generatedWorld;

  public WorldRoom CurrentRoom =>
      currentRoom;

  public RoomTransitionState State =>
      state;

  public bool IsTransitioning =>
      state != RoomTransitionState.Idle;

  public void Initialize(
      GeneratedWorld world)
  {
    generatedWorld = world;

    currentRoom =
        FindStartRoom(world);

    state =
        RoomTransitionState.Idle;
  }

  public void Transition(
      RoomConnection connection)
  {
    if (IsTransitioning)
      return;

    if (connection == null)
      return;

    if (generatedWorld == null)
    {
      Debug.LogWarning(
          "RoomTransitionController has no GeneratedWorld.");

      return;
    }

    if (currentRoom == null)
    {
      Debug.LogWarning(
          "RoomTransitionController has no current room.");

      return;
    }

    if (connection.From != currentRoom)
    {
      Debug.LogWarning(
          "Room transition rejected because the connection " +
          "does not originate from the current room.");

      return;
    }

    GeneratedRoom targetRoom =
        generatedWorld.GetRoom(
            connection.To);

    if (targetRoom == null)
    {
      Debug.LogWarning(
          "Could not find target generated room.");

      return;
    }

    StartCoroutine(
        PerformTransition(
            connection,
            targetRoom));
  }

  private IEnumerator PerformTransition(
      RoomConnection connection,
      GeneratedRoom targetRoom)
  {
    state =
        RoomTransitionState.Exiting;

    LockPlayer();

    if (transitionFader != null)
    {
      yield return transitionFader
          .FadeOutRoutine(
              fadeOutDuration);
    }

    currentRoom =
        connection.To;

    state =
        RoomTransitionState.Entering;

    MovePlayer(
        connection.EntryPosition);

    if (transitionFader != null)
    {
      yield return transitionFader
          .FadeInRoutine(
              fadeInDuration);
    }

    UnlockPlayer();

    state =
        RoomTransitionState.Idle;
  }

  private void MovePlayer(
      Vector2 position)
  {
    if (player == null)
    {
      Debug.LogWarning(
          "RoomTransitionController has no player.");

      return;
    }

    Rigidbody2D body =
        player.GetComponent<Rigidbody2D>();

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

  private void LockPlayer()
  {
    if (player == null)
      return;

    PlayerInput playerInput =
        player.GetComponent<PlayerInput>();

    if (playerInput != null)
      playerInput.enabled = false;

    Rigidbody2D body =
        player.GetComponent<Rigidbody2D>();

    if (body != null)
    {
      body.linearVelocity =
          Vector2.zero;

      body.angularVelocity =
          0f;
    }
  }

  private void UnlockPlayer()
  {
    if (player == null)
      return;

    PlayerInput playerInput =
        player.GetComponent<PlayerInput>();

    if (playerInput != null)
      playerInput.enabled = true;
  }

  private WorldRoom FindStartRoom(
      GeneratedWorld world)
  {
    if (world == null)
      return null;

    foreach (GeneratedRoom generatedRoom
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