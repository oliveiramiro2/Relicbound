using UnityEngine;

public class WorldRoomLayout
{
  public WorldRoom Room { get; }
  public WorldRoomTemplate Template { get; }
  public Vector2 Position { get; }
  public Vector2 Size { get; }

  public WorldRoomLayout(
      WorldRoom room,
      WorldRoomTemplate template,
      Vector2 position,
      Vector2 size
  )
  {
    Room = room;
    Template = template;
    Position = position;
    Size = size;
  }
}