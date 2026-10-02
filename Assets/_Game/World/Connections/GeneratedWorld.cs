using System.Collections.Generic;

public class GeneratedWorld
{
  private readonly List<GeneratedRoom> rooms =
      new();

  public IReadOnlyList<GeneratedRoom> Rooms =>
      rooms;

  public void AddRoom(
      GeneratedRoom room
  )
  {
    if (room == null)
      return;

    rooms.Add(room);
  }

  public GeneratedRoom GetRoom(
      WorldRoom room
  )
  {
    if (room == null)
      return null;

    foreach (
        GeneratedRoom generatedRoom
        in rooms)
    {
      if (generatedRoom.Room == room)
        return generatedRoom;
    }

    return null;
  }
}