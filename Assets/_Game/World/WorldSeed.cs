using UnityEngine;

public class WorldSeed : MonoBehaviour
{
  [SerializeField] private int seed;

  public int Seed => seed;

  public void GenerateNewSeed()
  {
    seed = Random.Range(
        int.MinValue,
        int.MaxValue
    );
  }

  public void SetSeed(int value)
  {
    seed = value;
  }
}