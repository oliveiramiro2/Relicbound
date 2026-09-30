using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Room Template",
    fileName = "RoomTemplate"
)]
public class WorldRoomTemplate : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private WorldRoomType roomType;

    [Header("Layout")]
    [SerializeField] private Vector2 size = new Vector2(10f, 6f);

    [Header("Content")]
    [SerializeField] private GameObject prefab;

    public WorldRoomType RoomType =>
        roomType;

    public Vector2 Size =>
        size;

    public GameObject Prefab =>
        prefab;
}