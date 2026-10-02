using UnityEngine;

public class RoomConnection
{
  public WorldRoom From { get; }

  public WorldRoom To { get; }

  public Vector2 ExitPosition { get; }

  public Vector2 EntryPosition { get; }

  public RoomConnection(
      WorldRoom from,
      WorldRoom to,
      Vector2 exitPosition,
      Vector2 entryPosition
  )
  {
    From = from;
    To = to;
    ExitPosition = exitPosition;
    EntryPosition = entryPosition;
  }
}