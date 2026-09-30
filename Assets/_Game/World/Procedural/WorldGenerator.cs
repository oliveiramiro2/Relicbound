using System;
using System.Collections.Generic;

public class WorldGenerator
{
  public WorldRoomGraph Generate(int seed)
  {
    System.Random random =
        new System.Random(seed);

    WorldRoomGraph graph =
        new WorldRoomGraph();

    List<WorldRoom> mainPath =
        new List<WorldRoom>();

    WorldRoom start =
        graph.CreateRoom(
            WorldRoomType.Start
        );

    mainPath.Add(start);

    WorldRoom current =
        start;

    int mainPathLength =
        random.Next(4, 7);

    for (int i = 1; i < mainPathLength; i++)
    {
      WorldRoomType roomType =
          i == mainPathLength - 1
              ? WorldRoomType.Exit
              : WorldRoomType.Normal;

      WorldRoom next =
          graph.CreateRoom(
              roomType
          );

      graph.Connect(
          current,
          next
      );

      mainPath.Add(next);

      current = next;
    }

    int branchCount =
        random.Next(1, 3);

    for (int i = 0; i < branchCount; i++)
    {
      CreateBranch(
          graph,
          mainPath,
          random
      );
    }

    return graph;
  }

  private void CreateBranch(
    WorldRoomGraph graph,
    List<WorldRoom> mainPath,
    System.Random random
)
  {
    if (mainPath.Count <= 1)
      return;

    int parentIndex =
        random.Next(
            1,
            mainPath.Count
        );

    WorldRoom parent =
        mainPath[parentIndex];

    WorldRoom branch =
        graph.CreateRoom(
            WorldRoomType.Branch
        );

    graph.Connect(
        parent,
        branch
    );
  }
}