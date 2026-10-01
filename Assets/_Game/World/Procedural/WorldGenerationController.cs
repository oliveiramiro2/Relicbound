using UnityEngine;

public class WorldGenerationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldSeed worldSeed;
    [SerializeField] private WorldRoomTemplateDatabase database;
    [SerializeField] private Transform worldRoot;

    private void Start()
    {
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        if (worldSeed == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "WorldSeed is missing."
            );

            return;
        }

        if (database == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "Room Template Database is missing."
            );

            return;
        }

        if (worldRoot == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "World Root is missing."
            );

            return;
        }

        WorldGenerator worldGenerator =
            new WorldGenerator();

        WorldRoomGraph graph =
            worldGenerator.Generate(
                worldSeed.Seed
            );

        WorldLayoutGenerator layoutGenerator =
            new WorldLayoutGenerator(
                database
            );

        WorldLayout layout =
            layoutGenerator.Generate(
                graph,
                worldSeed.Seed
            );

        WorldBuilder builder =
            new WorldBuilder(
                worldRoot
            );

        builder.Build(
            layout,
            worldSeed.Seed
        );
    }
}