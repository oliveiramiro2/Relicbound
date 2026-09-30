public class WorldRoomLayout
{
  public WorldRoom Room { get; }

  public WorldRoomTemplate Template { get; }

  public UnityEngine.Vector2 Position { get; }

  public WorldRoomLayout(
      WorldRoom room,
      WorldRoomTemplate template,
      UnityEngine.Vector2 position
  )
  {
    Room = room;
    Template = template;
    Position = position;
  }
}