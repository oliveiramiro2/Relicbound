using UnityEngine;

public class WorldLayoutDebug : MonoBehaviour
{
  [SerializeField] private int seed = 54321;

  private void Start()
  {
    WorldGenerator worldGenerator =
        new WorldGenerator();

    WorldRoomGraph graph =
        worldGenerator.Generate(seed);

    WorldLayoutGenerator layoutGenerator =
        new WorldLayoutGenerator();

    WorldLayout layout =
        layoutGenerator.Generate(graph);

    Debug.Log(
        $"Seed: {seed}"
    );

    foreach (
        WorldRoomLayout roomLayout
        in layout.Rooms
    )
    {
      WorldRoom room =
          roomLayout.Room;

      Vector2 position =
          roomLayout.Position;

      Debug.Log(
          $"Room {room.Id} " +
          $"[{room.Type}] " +
          $"Position: {position}"
      );
    }
  }
}