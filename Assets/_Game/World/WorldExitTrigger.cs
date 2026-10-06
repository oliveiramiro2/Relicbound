using UnityEngine;

public class WorldExitTrigger : MonoBehaviour
{
  private WorldGenerationController worldGenerationController;

  private bool activated;

  public void Initialize(
      WorldGenerationController controller
  )
  {
    worldGenerationController = controller;
    activated = false;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (activated)
      return;

    PlayerRelicCollector collector =
        other.GetComponent<PlayerRelicCollector>();

    if (collector == null)
      return;

    activated = true;

    worldGenerationController.GenerateNewWorld();
  }
}