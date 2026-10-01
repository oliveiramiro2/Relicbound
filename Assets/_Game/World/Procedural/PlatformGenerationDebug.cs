using UnityEngine;

public class PlatformGenerationDebug : MonoBehaviour
{
    [SerializeField] private int seed = 54321;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private RoomGenerationBounds bounds;
    [SerializeField] private Transform generatedContent;
    [SerializeField] private int platformCount = 5;

    private void Start()
    {
        System.Random random =
            new System.Random(seed);

        PlatformGenerator generator =
            new PlatformGenerator(
                platformPrefab
            );

        generator.Generate(
            bounds,
            random,
            generatedContent,
            platformCount
        );
    }
}