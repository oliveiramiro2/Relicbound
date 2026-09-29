using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicSlotView : MonoBehaviour
{
  [SerializeField] private Image icon;
  [SerializeField] private TMP_Text displayName;
  [SerializeField] private TMP_Text description;
  [SerializeField] private GameObject equippedIndicator;
  [SerializeField] private Button button;

  private Relic relic;

  public event Action<Relic> Clicked;

  public void Setup(
      Relic relic,
      RelicData data,
      bool isEquipped
  )
  {
    if (relic == null || data == null)
      return;

    this.relic = relic;

    icon.sprite = data.Icon;
    displayName.text = data.DisplayName;
    description.text = data.Description;

    equippedIndicator.SetActive(isEquipped);

    button.onClick.RemoveAllListeners();
    button.onClick.AddListener(OnClicked);
  }

  private void OnClicked()
  {
    Clicked?.Invoke(relic);
  }
}