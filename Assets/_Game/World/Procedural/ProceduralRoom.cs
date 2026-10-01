using UnityEngine;

public class ProceduralRoom : MonoBehaviour
{
  [Header("Generation")]
  [SerializeField] private RoomGenerationBounds bounds;
  [SerializeField] private Transform generatedContent;

  [Header("Platforms")]
  [SerializeField] private GameObject platformPrefab;
  [SerializeField] private int platformCount = 5;
  [SerializeField] private float minimumPlatformSpacing = 1f;

  public void Generate(
      int seed,
      int roomId
  )
  {
    if (bounds == null)
    {
      Debug.LogError(
          $"ProceduralRoom on {name}: " +
          "Generation Bounds is missing."
      );

      return;
    }

    if (generatedContent == null)
    {
      Debug.LogError(
          $"ProceduralRoom on {name}: " +
          "Generated Content is missing."
      );

      return;
    }

    if (platformPrefab == null)
    {
      Debug.LogError(
          $"ProceduralRoom on {name}: " +
          "Platform Prefab is missing."
      );

      return;
    }

    if (platformCount <= 0)
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
        platformCount,
        minimumPlatformSpacing
    );
  }
}