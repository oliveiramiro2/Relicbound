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

    [SerializeField]
    private GameObject roomExitPrefab;

    [SerializeField]
    private ProceduralRoomMarkerSettings
        markerSettings;

    [SerializeField]
    private Vector2 roomTransitionTriggerSize =
    new Vector2(
        2f,
        2f
    );

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

        generatedWorld = builder.GeneratedWorld;

        WorldRoomExitGenerator exitGenerator =
            new WorldRoomExitGenerator(
                roomExitPrefab,
                markerSettings.ConnectionExitHorizontalOffset,
                markerSettings.ConnectionExitVerticalOffset
            );

        exitGenerator.Generate(
            generatedWorld
        );

        WorldRoomConnectionGenerator
            connectionGenerator =
            new WorldRoomConnectionGenerator();

        RoomConnectionResult
            connectionResult =
            connectionGenerator.Generate(
                generatedWorld
            );

        RoomConnectionTriggerGenerator
            triggerGenerator =
            new RoomConnectionTriggerGenerator(
                roomTransitionTriggerSize
            );

        triggerGenerator.Generate(
            connectionResult,
            worldRoot
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