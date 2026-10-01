using System;
using UnityEngine;

public class PlatformGenerationDebug : MonoBehaviour
{
  [SerializeField] private int seed = 54321;
  [SerializeField] private GameObject platformPrefab;
  [SerializeField] private RoomGenerationBounds bounds;
  [SerializeField] private Transform generatedContent;

  private void Start()
  {
    System.Random random =
        new System.Random(seed);

    PlatformGenerator generator =
        new PlatformGenerator(
            platformPrefab,
            new Vector2(2f, 0.5f)
        );

    GameObject platform =
        generator.Generate(
            bounds,
            random,
            generatedContent
        );

    if (platform != null)
    {
      Debug.Log(
          $"Generated platform at: " +
          $"{platform.transform.position}"
      );
    }
  }
}