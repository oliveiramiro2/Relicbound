using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Extra Dash Relic",
    fileName = "ExtraDashRelic"
)]
public class ExtraDashRelicData : RelicData
{
  public override string Id =>
      "relic.extra_dash";

  public override Relic CreateRuntimeRelic()
  {
    return new ExtraDashRelic();
  }
}