using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Sped Relic",
    fileName = "SpeedRelic"
)]
public class SpeedRelicData : RelicData
{
  public override Relic CreateRuntimeRelic()
  {
    return new SpeedRelic();
  }
}