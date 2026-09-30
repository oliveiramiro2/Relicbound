using UnityEngine;

public class WorldBuilder
{
  private readonly Transform worldRoot;

  public WorldBuilder(
      Transform worldRoot
  )
  {
    this.worldRoot = worldRoot;
  }

  public void Build(
      WorldLayout layout
  )
  {
    if (layout == null)
      return;

    foreach (WorldRoomLayout roomLayout in layout.Rooms)
    {
      if (roomLayout.Template == null)
        continue;

      GameObject prefab =
          roomLayout.Template.Prefab;

      if (prefab == null)
        continue;

      Object.Instantiate(
          prefab,
          roomLayout.Position,
          Quaternion.identity,
          worldRoot
      );
    }
  }
}