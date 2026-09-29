using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicSlotView : MonoBehaviour
{
  [SerializeField] private Image icon;
  [SerializeField] private TMP_Text displayName;
  [SerializeField] private TMP_Text description;
  [SerializeField] private GameObject equippedIndicator;

  public void Setup(
      RelicData data,
      bool isEquipped
  )
  {
    if (data == null)
      return;

    icon.sprite = data.Icon;
    displayName.text = data.DisplayName;
    description.text = data.Description;

    equippedIndicator.SetActive(isEquipped);
  }
}