using UnityEngine;

public class WorldRoomLayout
{
  public WorldRoom Room { get; }

  public Vector2 Position { get; }

  public WorldRoomLayout(
      WorldRoom room,
      Vector2 position
  )
  {
    Room = room;
    Position = position;
  }
}