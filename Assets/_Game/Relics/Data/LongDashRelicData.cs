using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Long Dash Relic",
    fileName = "LongDashRelic"
)]
public class LongDashRelicData : RelicData
{
  public override string Id =>
      "relic.long_dash";

  public override Relic CreateRuntimeRelic()
  {
    return new LongDashRelic();
  }
}