using System.Collections.Generic;

public class PlatformGenerationResult
{
  public PlatformGraph Graph { get; }

  public int GeneratedCount =>
      Graph.Platforms.Count;

  public GeneratedPlatform StartPlatform { get; }

  public GeneratedPlatform ExitPlatform { get; }

  public IReadOnlyList<
      GeneratedPlatform
  > MainPathPlatforms
  { get; }

  public IReadOnlyList<
      GeneratedPlatform
  > BranchPlatforms
  { get; }

  public IReadOnlyList<
      GeneratedPlatform
  > DeadEndPlatforms
  { get; }

  public PlatformGenerationResult(
      PlatformGraph graph,
      GeneratedPlatform startPlatform,
      GeneratedPlatform exitPlatform,
      IReadOnlyList<GeneratedPlatform> mainPathPlatforms,
      IReadOnlyList<GeneratedPlatform> branchPlatforms,
      IReadOnlyList<GeneratedPlatform> deadEndPlatforms
  )
  {
    Graph =
        graph;

    StartPlatform =
        startPlatform;

    ExitPlatform =
        exitPlatform;

    MainPathPlatforms =
        mainPathPlatforms;

    BranchPlatforms =
        branchPlatforms;

    DeadEndPlatforms =
        deadEndPlatforms;
  }
}