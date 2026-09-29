using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/High Jump Relic",
    fileName = "HighJumpRelic"
)]
public class HighJumpRelicData : RelicData
{
  public override string Id => "relic.high_jump";

  public override Relic CreateRuntimeRelic()
  {
    return new HighJumpRelic();
  }
}