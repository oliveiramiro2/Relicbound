using UnityEngine;

public class ProceduralHazard : MonoBehaviour
{
  [SerializeField]
  private Vector2 size =
      new Vector2(
          1f,
          0.5f
      );

  public Vector2 Size =>
      size;
}