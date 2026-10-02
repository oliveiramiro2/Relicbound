using System;
using System.Collections.Generic;
using UnityEngine;

public class HazardGenerator
{
  private readonly GameObject hazardPrefab;

  private readonly Vector2 hazardSize;

  public HazardGenerator(
      GameObject hazardPrefab
  )
  {
    this.hazardPrefab =
        hazardPrefab;

    hazardSize =
        Vector2.zero;

    if (hazardPrefab == null)
      return;

    ProceduralHazard hazard =
        hazardPrefab.GetComponent<
            ProceduralHazard>();

    if (hazard == null)
      return;

    hazardSize =
        hazard.Size;
  }

  public HazardGenerationResult Generate(
      PlatformGraph graph,
      Transform parent,
      System.Random random,
      int maximumHazards,
      float minimumDistanceFromPlatformEdge
  )
  {
    List<GeneratedHazard>
        hazards =
        new List<GeneratedHazard>();

    if (!ValidateInput(
            graph,
            parent,
            random,
            maximumHazards))
    {
      return new HazardGenerationResult(
          hazards
      );
    }

    if (!HasValidHazardSize())
    {
      return new HazardGenerationResult(
          hazards
      );
    }

    List<GeneratedPlatform>
        candidates =
        new List<GeneratedPlatform>(
            graph.Platforms
        );

    Shuffle(
        candidates,
        random
    );

    foreach (
        GeneratedPlatform platform
        in candidates)
    {
      if (hazards.Count >=
          maximumHazards)
      {
        break;
      }

      if (platform.IsStart)
        continue;

      if (platform.IsExit)
        continue;

      if (platform.RouteType ==
          PlatformRouteType.DeadEnd)
      {
        continue;
      }

      if (TryGenerateHazard(
              platform,
              parent,
              random,
              minimumDistanceFromPlatformEdge,
              out GeneratedHazard hazard))
      {
        hazards.Add(
            hazard
        );
      }
    }

    return new HazardGenerationResult(
        hazards
    );
  }

  private bool TryGenerateHazard(
      GeneratedPlatform platform,
      Transform parent,
      System.Random random,
      float minimumDistanceFromPlatformEdge,
      out GeneratedHazard hazard
  )
  {
    hazard =
        null;

    float left =
        platform.Bounds.xMin +
        hazardSize.x / 2f +
        minimumDistanceFromPlatformEdge;

    float right =
        platform.Bounds.xMax -
        hazardSize.x / 2f -
        minimumDistanceFromPlatformEdge;

    if (left > right)
      return false;

    float x =
        Mathf.Lerp(
            left,
            right,
            (float)random.NextDouble()
        );

    float y =
        platform.Bounds.yMax +
        hazardSize.y / 2f;

    Vector2 position =
        new Vector2(
            x,
            y
        );

    Rect bounds =
        new Rect(
            position.x -
                hazardSize.x / 2f,

            position.y -
                hazardSize.y / 2f,

            hazardSize.x,
            hazardSize.y
        );

    UnityEngine.Object.Instantiate(
        hazardPrefab,
        position,
        Quaternion.identity,
        parent
    );

    hazard =
        new GeneratedHazard(
            platform,
            position,
            bounds
        );

    return true;
  }

  private void Shuffle<T>(
      List<T> list,
      System.Random random
  )
  {
    for (
        int i = list.Count - 1;
        i > 0;
        i--
    )
    {
      int index =
          random.Next(
              i + 1
          );

      T temporary =
          list[i];

      list[i] =
          list[index];

      list[index] =
          temporary;
    }
  }

  private bool ValidateInput(
      PlatformGraph graph,
      Transform parent,
      System.Random random,
      int maximumHazards
  )
  {
    if (graph == null)
      return false;

    if (parent == null)
      return false;

    if (random == null)
      return false;

    if (hazardPrefab == null)
      return false;

    if (maximumHazards <= 0)
      return false;

    return true;
  }

  private bool HasValidHazardSize()
  {
    if (hazardSize.x > 0f &&
        hazardSize.y > 0f)
    {
      return true;
    }

    Debug.LogError(
        "HazardGenerator: " +
        "Hazard prefab has no valid " +
        "ProceduralHazard size."
    );

    return false;
  }
}