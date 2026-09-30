using System.Collections.Generic;
using UnityEngine;

public class WorldLayoutGenerator
{
  private const float HorizontalSpacing = 10f;
  private const float BranchVerticalOffset = 6f;
  Dictionary<WorldRoom, int> branchCounts =
    new Dictionary<WorldRoom, int>();

  Dictionary<WorldRoom, Vector2> positions =
    new Dictionary<WorldRoom, Vector2>();

  public WorldLayout Generate(
      WorldRoomGraph graph
  )
  {
    WorldLayout layout =
        new WorldLayout();

    Dictionary<WorldRoom, Vector2> positions =
        new Dictionary<WorldRoom, Vector2>();

    foreach (WorldRoom room in graph.Rooms)
    {
      Vector2 position =
          CalculatePosition(
              room,
              positions,
              branchCounts
          );

      positions.Add(
          room,
          position
      );

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
      WorldRoom room,
      Dictionary<WorldRoom, Vector2> positions,
      Dictionary<WorldRoom, int> branchCounts
  )
  {
    if (room.Type == WorldRoomType.Start)
    {
      return Vector2.zero;
    }

    WorldRoom parent =
        FindParent(room);

    if (parent == null)
    {
      return Vector2.zero;
    }

    if (!positions.TryGetValue(
            parent,
            out Vector2 parentPosition))
    {
      return Vector2.zero;
    }

    if (room.Type == WorldRoomType.Branch)
    {
      int branchIndex = 0;

      if (branchCounts.TryGetValue(
              parent,
              out int count))
      {
        branchIndex = count;
      }

      branchCounts[parent] =
          branchIndex + 1;

      float verticalOffset =
          branchIndex % 2 == 0
              ? BranchVerticalOffset
              : -BranchVerticalOffset;

      return parentPosition +
          new Vector2(
              0f,
              verticalOffset
          );
    }

    return parentPosition +
        new Vector2(
            HorizontalSpacing,
            0f
        );
  }

  private WorldRoom FindParent(
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