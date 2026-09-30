using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Room Template",
    fileName = "RoomTemplate"
)]
public class WorldRoomTemplate : ScriptableObject
{
  [Header("Identity")]
  [SerializeField] private WorldRoomType roomType;

  [Header("Content")]
  [SerializeField] private GameObject prefab;

  public WorldRoomType RoomType =>
      roomType;

  public GameObject Prefab =>
      prefab;
}