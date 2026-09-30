using System.Collections.Generic;

public class WorldRoomGraph
{
  private readonly List<WorldRoom> rooms = new();

  public IReadOnlyList<WorldRoom> Rooms =>
      rooms;

  public WorldRoom CreateRoom()
  {
    WorldRoom room =
        new WorldRoom(rooms.Count);

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