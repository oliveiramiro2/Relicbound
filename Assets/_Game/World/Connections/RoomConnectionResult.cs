using System.Collections.Generic;

public class RoomConnectionResult
{
  public IReadOnlyList<RoomConnection>
      Connections
  { get; }

  public int Count =>
      Connections.Count;

  public RoomConnectionResult(
      IReadOnlyList<RoomConnection> connections
  )
  {
    Connections =
        connections;
  }
}