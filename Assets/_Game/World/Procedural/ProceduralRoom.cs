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

  [Header("Platforms")]
  [SerializeField]
  private GameObject platformPrefab;

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
            settings.MaximumAttemptsPerPlatform
        );

    LogGenerationResult(
        result
    );
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

    if (platformPrefab == null)
    {
      Debug.LogError(
          $"ProceduralRoom on {name}: " +
          "Platform Prefab is missing."
      );

      return false;
    }

    return true;
  }
}