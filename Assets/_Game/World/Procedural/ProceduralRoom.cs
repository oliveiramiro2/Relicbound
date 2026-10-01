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

    PlatformGenerator platformGenerator =
        new PlatformGenerator(
            platformPrefab
        );

    platformGenerator.Generate(
        bounds,
        random,
        generatedContent,
        settings.PlatformCount,
        settings.MinimumPlatformSpacing,
        settings.MaximumHorizontalDistance,
        settings.MinimumVerticalDistance,
        settings.MaximumVerticalDistance,
        settings.MaximumAttemptsPerPlatform
    );
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