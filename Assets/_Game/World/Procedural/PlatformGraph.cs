using System.Collections.Generic;

public class PlatformGraph
{
  private readonly List<GeneratedPlatform> platforms =
      new List<GeneratedPlatform>();

  private readonly Dictionary<
      GeneratedPlatform,
      List<PlatformConnection>
  > connections =
      new Dictionary<
          GeneratedPlatform,
          List<PlatformConnection>
      >();

  public IReadOnlyList<GeneratedPlatform> Platforms =>
      platforms;

  public void AddPlatform(
      GeneratedPlatform platform
  )
  {
    if (platform == null)
      return;

    if (platforms.Contains(platform))
      return;

    platforms.Add(
        platform
    );

    connections.Add(
        platform,
        new List<PlatformConnection>()
    );
  }

  public void Connect(
      GeneratedPlatform first,
      GeneratedPlatform second,
      ReachabilityType type
  )
  {
    if (first == null ||
        second == null)
    {
      return;
    }

    if (!connections.ContainsKey(first))
      return;

    if (!connections.ContainsKey(second))
      return;

    PlatformConnection forward =
        new PlatformConnection(
            first,
            second,
            type
        );

    PlatformConnection backward =
        new PlatformConnection(
            second,
            first,
            type
        );

    if (!ContainsConnection(
            first,
            second))
    {
      connections[first].Add(
          forward
      );
    }

    if (!ContainsConnection(
            second,
            first))
    {
      connections[second].Add(
          backward
      );
    }
  }

  public IReadOnlyList<PlatformConnection>
      GetConnections(
          GeneratedPlatform platform
      )
  {
    if (platform == null)
    {
      return new List<PlatformConnection>();
    }

    if (!connections.TryGetValue(
            platform,
            out List<PlatformConnection> result))
    {
      return new List<PlatformConnection>();
    }

    return result;
  }

  private bool ContainsConnection(
      GeneratedPlatform from,
      GeneratedPlatform to
  )
  {
    foreach (
        PlatformConnection connection
        in connections[from])
    {
      if (connection.To == to)
        return true;
    }

    return false;
  }
}