using System.Collections.Generic;

public class HazardGenerationResult
{
  public IReadOnlyList<GeneratedHazard>
      Hazards
  { get; }

  public int Count =>
      Hazards.Count;

  public HazardGenerationResult(
      IReadOnlyList<GeneratedHazard> hazards
  )
  {
    Hazards =
        hazards;
  }
}