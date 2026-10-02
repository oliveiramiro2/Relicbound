using System.Collections.Generic;

public class RelicGenerationResult
{
  public IReadOnlyList<GeneratedRelic>
      Relics
  { get; }

  public int Count =>
      Relics.Count;

  public RelicGenerationResult(
      IReadOnlyList<GeneratedRelic> relics
  )
  {
    Relics = relics;
  }
}