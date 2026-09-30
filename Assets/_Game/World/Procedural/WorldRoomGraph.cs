using System.Collections.Generic;

public class WorldRoomGraph
{
  private readonly List<WorldRoom> rooms = new();

  public IReadOnlyList<WorldRoom> Rooms =>
      rooms;

  public WorldRoom CreateRoom(
      WorldRoomType type
  )
  {
    WorldRoom room =
        new WorldRoom(
            rooms.Count,
            type
        );

    rooms.Add(room);

    return room;
  }

  public void Connect(
      WorldRoom first,
      WorldRoom second
  )
  {
    if (first == null || second == null)
      return;

    first.Connect(second);
  }
}