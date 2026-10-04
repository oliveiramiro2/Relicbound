using UnityEngine;

public static class PlatformSafetyValidator
{
  public static bool IsSafeSpawnPlatform(
      GeneratedPlatform platform)
  {
    if (platform == null)
      return false;

    if (!platform.IsStart)
      return false;

    return platform.Bounds.size.x >= 1f;
  }
}