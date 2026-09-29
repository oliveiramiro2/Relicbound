using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Sped Relic",
    fileName = "SpeedRelic"
)]
public class SpeedRelicData : RelicData
{
  public override string Id => "relic.speed";

  public override Relic CreateRuntimeRelic()
  {
    return new SpeedRelic();
  }
}