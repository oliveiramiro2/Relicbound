using UnityEngine;

public class GrappleVisualizer : MonoBehaviour
{
  [SerializeField] private GameObject targetIndicator;

  public void ShowTarget(Vector2 position)
  {
    targetIndicator.SetActive(true);
    targetIndicator.transform.position = position;
  }

  public void HideTarget()
  {
    targetIndicator.SetActive(false);
  }
}