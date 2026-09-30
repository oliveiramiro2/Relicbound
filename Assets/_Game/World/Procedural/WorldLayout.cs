using System.Collections.Generic;

public class WorldLayout
{
  private readonly List<WorldRoomLayout> rooms = new();

  public IReadOnlyList<WorldRoomLayout> Rooms =>
      rooms;

  public void AddRoom(WorldRoomLayout room)
  {
    if (room == null)
      return;

    rooms.Add(room);
  }
}