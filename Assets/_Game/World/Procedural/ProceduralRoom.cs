using UnityEngine;

public class ProceduralRoom : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField]
    private RoomGenerationBounds bounds;

    [SerializeField]
    private Transform generatedContent;

    [SerializeField]
    private ProceduralRoomSettings settings;

    [SerializeField]
    private MovementReachSettings reachSettings;

    [SerializeField]
    private ProceduralGrappleSettings
        grappleSettings;

    [Header("Platforms")]
    [SerializeField]
    private GameObject platformPrefab;

    [Header("Grapple")]
    [SerializeField]
    private GameObject grapplePointPrefab;

    [Header("Content")]
    [SerializeField]
    private ProceduralContentSettings
        contentSettings;

    [SerializeField]
    private GameObject hazardPrefab;

    [Header("Relics")]
    [SerializeField]
    private ProceduralRelicSettings relicSettings;

    [SerializeField]
    private RelicDatabase relicDatabase;

    [SerializeField]
    private GameObject relicPickupPrefab;

    [Header("Room Markers")]
    [SerializeField]
    private ProceduralRoomMarkerSettings
    markerSettings;

    [SerializeField]
    private GameObject playerSpawnPrefab;

    [SerializeField]
    private GameObject roomEntryPrefab;

    private RoomMarkerGenerationResult markerResult;

    public RoomMarkerGenerationResult MarkerResult =>
        markerResult;

    public void Generate(
        int seed,
        int roomId
    )
    {
        if (!ValidateReferences())
            return;

        int roomSeed =
            RoomSeedUtility.CreateSeed(
                seed,
                roomId
            );

        System.Random random =
            new System.Random(
                roomSeed
            );

        MovementReachProfile reachProfile =
            reachSettings.CreateDefaultProfile();

        PlatformGenerator platformGenerator =
            new PlatformGenerator(
                platformPrefab
            );

        PlatformGenerationResult result =
            platformGenerator.Generate(
                bounds,
                random,
                generatedContent,
                settings.PlatformCount,
                settings.MinimumPlatformSpacing,
                reachProfile,
                settings.MaximumAttemptsPerPlatform,
                settings.MinimumBranches,
                settings.MaximumBranches,
                settings.MinimumBranchLength,
                settings.MaximumBranchLength
            );

        if (grappleSettings.Enabled)
        {
            GenerateGrappleConnections(
                result,
                random
            );
        }

        GenerateContent(
            result,
            random
        );

        GenerateRelics(
            result,
            random
        );

        GenerateRoomMarkers(
            result
        );

        LogGenerationResult(
            result
        );
    }

    private void GenerateGrappleConnections(
        PlatformGenerationResult result,
        System.Random random
    )
    {
        if (result == null)
            return;

        GrappleConnectionGenerator generator =
            new GrappleConnectionGenerator(
                grapplePointPrefab,
                reachSettings
                    .GrappleDistance,
                grappleSettings
                    .MaximumConnections,
                grappleSettings
                    .DistanceFromPlatform
            );

        GrappleConnectionResult
            grappleResult =
            generator.Generate(
                result.Graph,
                generatedContent,
                random
            );

        Debug.Log(
            $"ProceduralRoom [{name}] " +
            $"generated " +
            $"{grappleResult.ConnectionCount} " +
            $"grapple connections."
        );
    }

    private void GenerateContent(
        PlatformGenerationResult result,
        System.Random random
    )
    {
        if (result == null)
            return;

        if (contentSettings == null)
            return;

        if (!contentSettings.GenerateHazards)
            return;

        HazardGenerator hazardGenerator =
            new HazardGenerator(
                hazardPrefab
            );

        HazardGenerationResult
            hazardResult =
            hazardGenerator.Generate(
                result.Graph,
                generatedContent,
                random,
                contentSettings
                    .MaximumHazardsPerRoom,
                contentSettings
                    .MinimumDistanceFromPlatformEdge
            );

        Debug.Log(
            $"ProceduralRoom [{name}] " +
            $"generated " +
            $"{hazardResult.Count} hazards."
        );
    }

    private void GenerateRelics(
    PlatformGenerationResult result,
    System.Random random
)
    {
        if (relicSettings == null)
            return;

        if (!relicSettings.GenerateRelics)
            return;

        if (relicDatabase == null)
        {
            Debug.LogWarning(
                "ProceduralRoom: " +
                "RelicDatabase is missing."
            );

            return;
        }

        if (relicPickupPrefab == null)
        {
            Debug.LogWarning(
                "ProceduralRoom: " +
                "Relic pickup prefab is missing."
            );

            return;
        }

        RelicGenerator generator =
            new RelicGenerator(
                relicPickupPrefab,
                relicDatabase,
                relicSettings.VerticalOffset
            );

        RelicGenerationResult
            generationResult =
            generator.Generate(
                result.Graph,
                generatedContent,
                random,
                relicSettings.MinimumRelicsPerRoom,
                relicSettings.MaximumRelicsPerRoom,
                relicSettings.PreferBranches,
                relicSettings.BranchSelectionWeight
            );

        Debug.Log(
            "ProceduralRoom: Generated " +
            generationResult.Count +
            " relic(s)."
        );
    }

    private void GenerateRoomMarkers(
        PlatformGenerationResult result
    )
    {
        if (markerSettings == null)
            return;

        RoomMarkerGenerator generator =
            new RoomMarkerGenerator(
                playerSpawnPrefab,
                roomEntryPrefab,
                markerSettings.PlayerSpawnVerticalOffset,
                markerSettings.RoomEntryVerticalOffset
            );

        markerResult =
            generator.Generate(
                result,
                generatedContent
            );

        if (markerResult.PlayerSpawn != null)
        {
            Debug.Log(
                "ProceduralRoom: " +
                "Player spawn generated at " +
                markerResult.PlayerSpawn.Position
            );
        }

        if (markerResult.RoomEntry != null)
        {
            Debug.Log(
                "ProceduralRoom: " +
                "Room entry generated at " +
                markerResult.RoomEntry.Position
            );
        }
    }

    private void LogGenerationResult(
        PlatformGenerationResult result
    )
    {
        if (result == null)
            return;

        Debug.Log(
            $"ProceduralRoom [{name}] " +
            $"generated {result.GeneratedCount} platforms."
        );

        Debug.Log(
            $"Main Path: " +
            $"{result.MainPathPlatforms.Count}"
        );

        Debug.Log(
            $"Branches: " +
            $"{result.BranchPlatforms.Count}"
        );

        Debug.Log(
            $"Dead Ends: " +
            $"{result.DeadEndPlatforms.Count}"
        );

        if (result.StartPlatform != null)
        {
            Debug.Log(
                $"Start Platform: " +
                $"{result.StartPlatform.Position}"
            );
        }

        if (result.ExitPlatform != null)
        {
            Debug.Log(
                $"Exit Platform: " +
                $"{result.ExitPlatform.Position}"
            );
        }
        else
        {
            Debug.LogWarning(
                $"ProceduralRoom [{name}] " +
                "did not generate a valid exit platform."
            );
        }
    }

    private bool ValidateReferences()
    {
        if (bounds == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Generation Bounds is missing."
            );

            return false;
        }

        if (generatedContent == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Generated Content is missing."
            );

            return false;
        }

        if (settings == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Procedural Room Settings is missing."
            );

            return false;
        }

        if (reachSettings == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Movement Reach Settings is missing."
            );

            return false;
        }

        if (grappleSettings == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Procedural Grapple Settings is missing."
            );

            return false;
        }

        if (platformPrefab == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Platform Prefab is missing."
            );

            return false;
        }

        if (grapplePointPrefab == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Grapple Point Prefab is missing."
            );

            return false;
        }

        if (contentSettings == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Procedural Content Settings " +
                "is missing."
            );

            return false;
        }

        if (hazardPrefab == null)
        {
            Debug.LogError(
                $"ProceduralRoom on {name}: " +
                "Hazard Prefab is missing."
            );

            return false;
        }

        return true;
    }
}