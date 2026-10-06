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

    [SerializeField]
    private RoomTransitionController roomTransitionController;

    [Header("World Exit")]
    [SerializeField]
    private GameObject worldExitPrefab;

    [SerializeField]
    private Vector2 worldExitTriggerSize =
        new Vector2(2f, 2f);

    [Header("Player")]
    [SerializeField]
    private GameObject player;

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

        if (roomTransitionController != null)
        {
            roomTransitionController.Initialize(
                generatedWorld
            );
        }

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
            worldRoot,
            roomTransitionController
        );

        WorldExitGenerator worldExitGenerator =
            new WorldExitGenerator(
                worldExitPrefab,
                worldExitTriggerSize
            );

        worldExitGenerator.Generate(
            generatedWorld,
            this,
            worldRoot
        );
    }

    private void ClearWorld()
    {
        if (worldRoot == null)
            return;

        for (int i = worldRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(
                worldRoot.GetChild(i).gameObject
            );
        }
    }

    public void GenerateNewWorld()
    {
        ClearWorld();

        if (worldSeed == null)
            return;


        worldSeed.GenerateNewSeed();
        GenerateWorld();
        PlacePlayerAtWorldStart();
    }

    private void PlacePlayerAtWorldStart()
    {
        if (player == null)
            return;

        if (generatedWorld == null)
            return;

        ResetPlayerMovement(player);
        player.GetComponent<Transform>().position =
            new(0, 5);
    }

    private void ResetPlayerMovement(GameObject player)
    {
        if (player == null)
            return;

        PlayerMovement movement =
            player.GetComponent<PlayerMovement>();

        if (movement == null)
            return;

        movement.ResetMovementState();
    }

    public GeneratedWorld GeneratedWorld =>
        generatedWorld;
}