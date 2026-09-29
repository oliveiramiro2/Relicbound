using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Upward Dash Relic",
    fileName = "UpwardDashRelic"
)]
public class UpwardDashRelicData : RelicData
{
  public override string Id =>
      "relic.upward_dash";

  public override Relic CreateRuntimeRelic()
  {
    return new UpwardDashRelic();
  }
}