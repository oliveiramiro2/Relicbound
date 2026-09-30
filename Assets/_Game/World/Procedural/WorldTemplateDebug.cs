using System;
using UnityEngine;

public class WorldTemplateDebug : MonoBehaviour
{
  [SerializeField] private int seed = 54321;
  [SerializeField] private WorldRoomTemplateDatabase database;

  private void Start()
  {
    WorldGenerator generator =
        new WorldGenerator();

    WorldRoomGraph graph =
        generator.Generate(seed);

    System.Random random =
        new System.Random(seed);

    WorldRoomTemplateSelector selector =
        new WorldRoomTemplateSelector(
            database
        );

    Debug.Log(
        $"Template selection - Seed: {seed}"
    );

    foreach (WorldRoom room in graph.Rooms)
    {
      WorldRoomTemplate template =
          selector.Select(
              room,
              random
          );

      if (template == null)
      {
        Debug.Log(
            $"Room {room.Id} [{room.Type}] " +
            "-> NO TEMPLATE"
        );

        continue;
      }

      Debug.Log(
          $"Room {room.Id} [{room.Type}] " +
          $"-> {template.name}"
      );
    }
  }
}