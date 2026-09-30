using UnityEngine;

public class WorldGraphDebug : MonoBehaviour
{
  private void Start()
  {
    WorldRoomGraph graph =
        new WorldRoomGraph();

    WorldRoom room0 =
        graph.CreateRoom();

    WorldRoom room1 =
        graph.CreateRoom();

    WorldRoom room2 =
        graph.CreateRoom();

    WorldRoom room3 =
        graph.CreateRoom();

    graph.Connect(room0, room1);
    graph.Connect(room1, room2);
    graph.Connect(room1, room3);

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