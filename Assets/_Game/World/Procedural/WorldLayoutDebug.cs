using UnityEngine;

public class WorldLayoutDebug : MonoBehaviour
{
  [SerializeField] private int seed = 54321;

  [Header("Visualization")]
  [SerializeField] private float roomSize = 1f;

  private WorldLayout layout;

  private void Start()
  {
    GenerateLayout();
  }

  private void GenerateLayout()
  {
    WorldGenerator worldGenerator =
        new WorldGenerator();

    WorldRoomGraph graph =
        worldGenerator.Generate(seed);

    WorldLayoutGenerator layoutGenerator =
        new WorldLayoutGenerator();

    layout =
        layoutGenerator.Generate(graph);

    Debug.Log(
        $"Generated world with seed: {seed}"
    );
  }

  private void OnDrawGizmos()
  {
    if (layout == null)
      return;

    DrawConnections();
    DrawRooms();
  }

  private void DrawConnections()
  {
    foreach (WorldRoomLayout roomLayout in layout.Rooms)
    {
      WorldRoom room =
          roomLayout.Room;

      Vector3 start =
          roomLayout.Position;

      foreach (WorldRoom connection in room.Connections)
      {
        WorldRoomLayout connectionLayout =
            FindLayout(connection);

        if (connectionLayout == null)
          continue;

        Vector3 end =
            connectionLayout.Position;

        Gizmos.DrawLine(
            start,
            end
        );
      }
    }
  }

  private void DrawRooms()
  {
    foreach (WorldRoomLayout roomLayout in layout.Rooms)
    {
      Vector3 position =
          roomLayout.Position;

      Gizmos.DrawWireCube(
          position,
          Vector3.one * roomSize
      );
    }
  }

  private WorldRoomLayout FindLayout(
      WorldRoom room
  )
  {
    foreach (WorldRoomLayout roomLayout in layout.Rooms)
    {
      if (roomLayout.Room == room)
        return roomLayout;
    }

    return null;
  }
}