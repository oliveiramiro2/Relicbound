using UnityEngine;

public class WorldBuilder
{
  private readonly Transform worldRoot;

  private GeneratedWorld generatedWorld;

  public GeneratedWorld GeneratedWorld =>
      generatedWorld;

  public WorldBuilder(
      Transform worldRoot
  )
  {
    this.worldRoot =
        worldRoot;

    generatedWorld =
        new GeneratedWorld();
  }

  public void Build(
      WorldLayout layout,
      int seed
  )
  {
    if (layout == null)
      return;

    if (worldRoot == null)
      return;

    generatedWorld =
        new GeneratedWorld();

    foreach (
        WorldRoomLayout roomLayout
        in layout.Rooms)
    {
      if (roomLayout.Template == null)
        continue;

      GameObject prefab =
          roomLayout.Template.Prefab;

      if (prefab == null)
        continue;

      GameObject roomObject =
          Object.Instantiate(
              prefab,
              roomLayout.Position,
              Quaternion.identity,
              worldRoot
          );

      ProceduralRoom proceduralRoom =
          roomObject.GetComponent<
              ProceduralRoom>();

      if (proceduralRoom == null)
      {
        Object.Destroy(
            roomObject
        );

        continue;
      }

      proceduralRoom.Generate(
          seed,
          roomLayout.Room.Id
      );

      GeneratedRoom generatedRoom =
          new GeneratedRoom(
              roomLayout.Room,
              roomObject,
              proceduralRoom.MarkerResult
          );

      generatedWorld.AddRoom(
          generatedRoom
      );
    }
  }
}