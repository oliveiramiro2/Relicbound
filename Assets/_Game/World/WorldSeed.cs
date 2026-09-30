using UnityEngine;

public class WorldSeed : MonoBehaviour
{
  [SerializeField] private int seed;

  public int Seed => seed;

  public void SetSeed(int value)
  {
    seed = value;
  }
}