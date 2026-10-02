using UnityEngine;

public class ProceduralRoomMarker : MonoBehaviour
{
  public enum MarkerType
  {
    PlayerSpawn,
    RoomExit
  }

  [SerializeField]
  private MarkerType type;

  public MarkerType Type =>
      type;
}