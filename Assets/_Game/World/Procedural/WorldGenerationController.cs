using UnityEngine;

public class WorldGenerationController :
    MonoBehaviour
{
    [SerializeField]
    private WorldSeed worldSeed;

    [SerializeField]
    private WorldRoomTemplateDatabase
        roomTemplateDatabase;

    [SerializeField]
    private Transform worldRoot;

    [SerializeField]
    private bool generateOnStart = true;

    [SerializeField]
    private RoomConnectionVisualizer
        connectionVisualizer;

    private GeneratedWorld generatedWorld;

    private void Start()
    {
        if (!generateOnStart)
            return;

        GenerateWorld();
    }

    public void GenerateWorld()
    {
        if (worldSeed == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "WorldSeed is missing."
            );

            return;
        }

        if (roomTemplateDatabase == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "WorldRoomTemplateDatabase " +
                "is missing."
            );

            return;
        }

        if (worldRoot == null)
        {
            Debug.LogError(
                "WorldGenerationController: " +
                "WorldRoot is missing."
            );

            return;
        }

        WorldGenerator generator =
            new WorldGenerator();

        WorldRoomGraph graph =
            generator.Generate(
                worldSeed.Seed
            );

        WorldLayoutGenerator
            layoutGenerator =
            new WorldLayoutGenerator(
                roomTemplateDatabase
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

        generatedWorld =
            builder.GeneratedWorld;

        WorldRoomConnectionGenerator
            connectionGenerator =
            new WorldRoomConnectionGenerator();

        RoomConnectionResult
            connectionResult =
            connectionGenerator.Generate(
                generatedWorld
            );

        if (connectionVisualizer != null)
        {
            connectionVisualizer.Visualize(
                connectionResult
            );
        }

        Debug.Log(
            "WorldGenerationController: " +
            "Generated " +
            generatedWorld.Rooms.Count +
            " room(s) and " +
            connectionResult.Count +
            " connection(s)."
        );
    }

    public GeneratedWorld GeneratedWorld =>
        generatedWorld;
}