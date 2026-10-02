using System.Collections.Generic;

public class GrappleConnectionResult
{
  public IReadOnlyList<GrappleConnection>
      Connections
  { get; }

  public int ConnectionCount =>
      Connections.Count;

  public GrappleConnectionResult(
      IReadOnlyList<GrappleConnection> connections
  )
  {
    Connections =
        connections;
  }
}