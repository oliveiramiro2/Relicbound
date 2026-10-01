using UnityEngine;

public class WorldBuilder
{
  private readonly Transform worldRoot;

  public WorldBuilder(
      Transform worldRoot
  )
  {
    this.worldRoot =
        worldRoot;
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
          roomObject.GetComponent<ProceduralRoom>();

      if (proceduralRoom == null)
        continue;

      proceduralRoom.Generate(
          seed,
          roomLayout.Room.Id
      );
    }
  }
}