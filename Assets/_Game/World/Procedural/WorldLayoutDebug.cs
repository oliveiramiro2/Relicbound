using UnityEngine;

public class WorldLayoutDebug : MonoBehaviour
{
  [SerializeField] private int seed = 54321;

  [Header("Visualization")]
  [SerializeField] private float roomSize = 1f;
  [SerializeField] private WorldRoomTemplateDatabase database;
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
        new WorldLayoutGenerator(
            database
        );

    layout =
        layoutGenerator.Generate(
            graph,
            seed
        );

    Debug.Log(
        $"Generated world with seed: {seed}"
    );

    foreach (WorldRoomLayout roomLayout in layout.Rooms)
    {
      WorldRoom room =
          roomLayout.Room;

      Vector2 position =
          roomLayout.Position;

      string templateName =
          roomLayout.Template != null
              ? roomLayout.Template.name
              : "NONE";

      Debug.Log(
          $"Room {room.Id} " +
          $"[{room.Type}] " +
          $"Position: {position} " +
          $"Template: {templateName}"
      );
    }
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