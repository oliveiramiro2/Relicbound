using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Relics/Relic Database",
    fileName = "RelicDatabase"
)]
public class RelicDatabase : ScriptableObject
{
  [SerializeField] private List<RelicData> relics = new();

  public RelicData GetById(string id)
  {
    if (string.IsNullOrEmpty(id))
      return null;

    foreach (RelicData relic in relics)
    {
      if (relic == null)
        continue;

      if (relic.Id == id)
        return relic;
    }

    return null;
  }
}