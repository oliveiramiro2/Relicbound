using System;

public class WorldGenerator
{
  public WorldRoomGraph Generate(int seed)
  {
    System.Random random =
        new System.Random(seed);

    WorldRoomGraph graph =
        new WorldRoomGraph();

    WorldRoom start =
        graph.CreateRoom();

    WorldRoom current =
        start;

    int roomCount =
        random.Next(4, 7);

    for (int i = 1; i < roomCount; i++)
    {
      WorldRoom next =
          graph.CreateRoom();

      graph.Connect(
          current,
          next
      );

      current = next;
    }

    return graph;
  }
}