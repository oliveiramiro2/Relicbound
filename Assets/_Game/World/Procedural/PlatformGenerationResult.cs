public class PlatformGenerationResult
{
  public PlatformGraph Graph { get; }

  public int GeneratedCount =>
      Graph.Platforms.Count;

  public GeneratedPlatform StartPlatform { get; }

  public GeneratedPlatform ExitPlatform { get; }

  public PlatformGenerationResult(
      PlatformGraph graph,
      GeneratedPlatform startPlatform,
      GeneratedPlatform exitPlatform
  )
  {
    Graph = graph;
    StartPlatform = startPlatform;
    ExitPlatform = exitPlatform;
  }
}