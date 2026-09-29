using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Double Jump Relic",
    fileName = "DoubleJumpRelic"
)]
public class DoubleJumpRelicData : RelicData
{
  public override string Id =>
      "relic.double_jump";

  public override Relic CreateRuntimeRelic()
  {
    return new DoubleJumpRelic();
  }
}