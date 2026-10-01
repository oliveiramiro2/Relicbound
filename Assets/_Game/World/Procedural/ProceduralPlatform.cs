using UnityEngine;

public class ProceduralPlatform : MonoBehaviour
{
  [SerializeField]
  private Vector2 size =
      new Vector2(2f, 0.5f);

  public Vector2 Size =>
      size;
}