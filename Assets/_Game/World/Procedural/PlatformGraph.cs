using System.Collections.Generic;

public class PlatformGraph
{
  private readonly List<GeneratedPlatform> platforms =
      new List<GeneratedPlatform>();

  private readonly Dictionary<
      GeneratedPlatform,
      List<GeneratedPlatform>
  > connections =
      new Dictionary<
          GeneratedPlatform,
          List<GeneratedPlatform>
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

    platforms.Add(platform);

    connections.Add(
        platform,
        new List<GeneratedPlatform>()
    );
  }

  public void Connect(
      GeneratedPlatform first,
      GeneratedPlatform second
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

    if (!connections[first].Contains(second))
    {
      connections[first].Add(second);
    }

    if (!connections[second].Contains(first))
    {
      connections[second].Add(first);
    }
  }

  public IReadOnlyList<GeneratedPlatform> GetConnections(
      GeneratedPlatform platform
  )
  {
    if (platform == null)
      return new List<GeneratedPlatform>();

    if (!connections.TryGetValue(
            platform,
            out List<GeneratedPlatform> result))
    {
      return new List<GeneratedPlatform>();
    }

    return result;
  }
}