using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
  [SerializeField]
  private Transform player;

  public void Spawn(
      Vector2 position
  )
  {
    if (player == null)
    {
      Debug.LogWarning(
          "PlayerSpawnController: " +
          "Player reference is missing."
      );

      return;
    }

    Rigidbody2D body =
        player.GetComponent<Rigidbody2D>();

    if (body != null)
    {
      body.linearVelocity =
          Vector2.zero;
    }

    player.position =
        position;
  }
}