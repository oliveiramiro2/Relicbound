using System.Collections.Generic;

public class WorldRoom
{
  public int Id { get; }

  public WorldRoomType Type { get; }

  public List<WorldRoom> Connections { get; } = new();

  public WorldRoom(
      int id,
      WorldRoomType type
  )
  {
    Id = id;
    Type = type;
  }

  public void Connect(WorldRoom room)
  {
    if (room == null)
      return;

    if (room == this)
      return;

    if (Connections.Contains(room))
      return;

    Connections.Add(room);

    if (!room.Connections.Contains(this))
    {
      room.Connections.Add(this);
    }
  }
}