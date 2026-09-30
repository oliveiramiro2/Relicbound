using UnityEngine;

public class WorldGeneratorDebug : MonoBehaviour
{
  [SerializeField] private int seed = 12345;

  private void Start()
  {
    WorldGenerator generator =
        new WorldGenerator();

    WorldRoomGraph graph =
        generator.Generate(seed);

    Debug.Log(
        $"Seed: {seed}"
    );

    Debug.Log(
        $"Room Count: {graph.Rooms.Count}"
    );

    foreach (WorldRoom room in graph.Rooms)
    {
      string connections = "";

      foreach (WorldRoom connection in room.Connections)
      {
        connections +=
            $" {connection.Id}";
      }

      Debug.Log(
          $"Room {room.Id} ->{connections}"
      );
    }
  }
}