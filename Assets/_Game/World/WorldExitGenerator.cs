using UnityEngine;

public class WorldExitGenerator
{
  private readonly GameObject exitPrefab;
  private readonly Vector2 triggerSize;

  public WorldExitGenerator(
      GameObject exitPrefab,
      Vector2 triggerSize
  )
  {
    this.exitPrefab = exitPrefab;
    this.triggerSize = triggerSize;
  }

  public void Generate(
      GeneratedWorld generatedWorld,
      WorldGenerationController controller,
      Transform parent
  )
  {
    if (generatedWorld == null)
      return;

    if (exitPrefab == null)
      return;

    if (controller == null)
      return;

    if (parent == null)
      return;

    GeneratedRoom exitRoom =
        FindExitRoom(generatedWorld);

    if (exitRoom == null)
      return;

    if (exitRoom.Markers == null)
      return;

    GeneratedPlatform exitPlatform =
        exitRoom.Markers.ExitPlatform;

    if (exitPlatform == null)
      return;

    Vector2 position =
        new Vector2(
            exitPlatform.Position.x,
            exitPlatform.Bounds.yMax + 2f
        );

    GameObject exitObject =
        Object.Instantiate(
            exitPrefab,
            position,
            Quaternion.identity,
            exitRoom.Instance.transform
        );

    WorldExitTrigger trigger =
        exitObject.GetComponent<WorldExitTrigger>();

    if (trigger == null)
    {
      Debug.LogError(
          "WorldExitGenerator: " +
          "Exit prefab requires " +
          "WorldExitTrigger."
      );

      Object.Destroy(exitObject);
      return;
    }

    trigger.Initialize(controller);

    BoxCollider2D collider =
        exitObject.GetComponent<BoxCollider2D>();

    if (collider != null)
      collider.size = triggerSize;
  }

  private GeneratedRoom FindExitRoom(
      GeneratedWorld generatedWorld
  )
  {
    foreach (GeneratedRoom room
             in generatedWorld.Rooms)
    {
      if (room == null)
        continue;

      if (room.Room == null)
        continue;

      if (room.Room.Type != WorldRoomType.Exit)
        continue;

      return room;
    }

    return null;
  }
}