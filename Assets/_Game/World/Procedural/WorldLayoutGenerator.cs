using UnityEngine;

public class WorldLayoutGenerator
{
  private const float HorizontalSpacing = 10f;
  private const float BranchVerticalOffset = 6f;

  public WorldLayout Generate(
      WorldRoomGraph graph
  )
  {
    WorldLayout layout =
        new WorldLayout();

    foreach (WorldRoom room in graph.Rooms)
    {
      Vector2 position =
          CalculatePosition(room);

      WorldRoomLayout roomLayout =
          new WorldRoomLayout(
              room,
              position
          );

      layout.AddRoom(roomLayout);
    }

    return layout;
  }

  private Vector2 CalculatePosition(
      WorldRoom room
  )
  {
    if (room.Type == WorldRoomType.Start)
    {
      return new Vector2(
          0f,
          0f
      );
    }

    WorldRoom parent =
        FindMainPathParent(room);

    if (parent == null)
    {
      return Vector2.zero;
    }

    return new Vector2(
        parent.Id * HorizontalSpacing,
        room.Type == WorldRoomType.Branch
            ? BranchVerticalOffset
            : 0f
    );
  }

  private WorldRoom FindMainPathParent(
      WorldRoom room
  )
  {
    foreach (WorldRoom connection in room.Connections)
    {
      if (connection.Id < room.Id)
        return connection;
    }

    return null;
  }
}