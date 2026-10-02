using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Marker Settings",
    fileName = "ProceduralRoomMarkerSettings"
)]
public class ProceduralRoomMarkerSettings :
    ScriptableObject
{
  [Header("Placement")]
  [SerializeField]
  private float verticalOffset = 1f;

  [SerializeField]
  private float exitVerticalOffset = 1f;

  public float VerticalOffset =>
      Mathf.Max(
          0f,
          verticalOffset
      );

  public float ExitVerticalOffset =>
      Mathf.Max(
          0f,
          exitVerticalOffset
      );
}