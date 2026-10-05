using UnityEngine;

public class RoomGenerationBounds : MonoBehaviour
{
  [SerializeField]
  private Vector2 size =
      new Vector2(25f, 20f);

  public Vector2 Size =>
      size;

  public Vector2 Center =>
      transform.position;

  public float MinX =>
      Center.x - Size.x / 2f;

  public float MaxX =>
      Center.x + Size.x / 2f;

  public float MinY =>
      Center.y - Size.y / 2f;

  public float MaxY =>
      Center.y + Size.y / 2f;

  private void OnDrawGizmos()
  {
    Gizmos.DrawWireCube(
        Center,
        Size
    );
  }
}