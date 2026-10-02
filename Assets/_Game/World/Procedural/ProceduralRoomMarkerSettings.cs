using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Marker Settings",
    fileName = "ProceduralRoomMarkerSettings"
)]
public class ProceduralRoomMarkerSettings :
    ScriptableObject
{
  [Header("Player Spawn")]
  [SerializeField]
  private float playerSpawnVerticalOffset = 1f;

  [Header("Room Entry")]
  [SerializeField]
  private float roomEntryVerticalOffset = 1f;

  [Header("Room Exit")]
  [SerializeField]
  private float roomExitVerticalOffset = 1f;

  public float PlayerSpawnVerticalOffset =>
      Mathf.Max(
          0f,
          playerSpawnVerticalOffset
      );

  public float RoomEntryVerticalOffset =>
      Mathf.Max(
          0f,
          roomEntryVerticalOffset
      );

  public float RoomExitVerticalOffset =>
      Mathf.Max(
          0f,
          roomExitVerticalOffset
      );
}