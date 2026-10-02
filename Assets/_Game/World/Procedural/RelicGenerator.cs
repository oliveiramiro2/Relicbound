using System;
using System.Collections.Generic;
using UnityEngine;

public class RelicGenerator
{
  private readonly GameObject relicPickupPrefab;
  private readonly RelicDatabase database;
  private readonly float verticalOffset;

  public RelicGenerator(
      GameObject relicPickupPrefab,
      RelicDatabase database,
      float verticalOffset
  )
  {
    this.relicPickupPrefab =
        relicPickupPrefab;

    this.database =
        database;

    this.verticalOffset =
        Mathf.Max(
            0f,
            verticalOffset
        );
  }

  public RelicGenerationResult Generate(
      PlatformGraph graph,
      Transform parent,
      System.Random random,
      int minimumRelics,
      int maximumRelics,
      bool preferBranches,
      float branchSelectionWeight
  )
  {
    List<GeneratedRelic>
        generatedRelics =
        new List<GeneratedRelic>();

    if (!ValidateInput(
            graph,
            parent,
            random,
            minimumRelics,
            maximumRelics))
    {
      return new RelicGenerationResult(
          generatedRelics
      );
    }

    List<RelicData> availableRelics =
        GetAvailableRelics();

    if (availableRelics.Count == 0)
    {
      Debug.LogWarning(
          "RelicGenerator: " +
          "RelicDatabase contains no valid relics."
      );

      return new RelicGenerationResult(
          generatedRelics
      );
    }

    int targetCount =
        random.Next(
            minimumRelics,
            maximumRelics + 1
        );

    List<GeneratedPlatform> candidates =
        BuildCandidates(
            graph,
            preferBranches,
            branchSelectionWeight,
            random
        );

    if (candidates.Count == 0)
    {
      return new RelicGenerationResult(
          generatedRelics
      );
    }

    int count =
        Mathf.Min(
            targetCount,
            candidates.Count
        );

    for (
        int i = 0;
        i < count;
        i++)
    {
      GeneratedPlatform platform =
          candidates[i];

      RelicData relicData =
          availableRelics[
              random.Next(
                  availableRelics.Count
              )
          ];

      Vector2 position =
          CalculatePosition(
              platform
          );

      GameObject instance =
          UnityEngine.Object.Instantiate(
              relicPickupPrefab,
              position,
              Quaternion.identity,
              parent
          );

      RelicPickup pickup =
          instance.GetComponent<
              RelicPickup>();

      if (pickup == null)
      {
        UnityEngine.Object.Destroy(
            instance
        );

        Debug.LogError(
            "RelicGenerator: " +
            "Relic pickup prefab does not " +
            "contain a RelicPickup component."
        );

        continue;
      }

      pickup.Initialize(
          relicData
      );

      GeneratedRelic generatedRelic =
          new GeneratedRelic(
              platform,
              relicData,
              position
          );

      generatedRelics.Add(
          generatedRelic
      );
    }

    return new RelicGenerationResult(
        generatedRelics
    );
  }

  private List<GeneratedPlatform>
      BuildCandidates(
          PlatformGraph graph,
          bool preferBranches,
          float branchSelectionWeight,
          System.Random random
      )
  {
    List<GeneratedPlatform>
        branchCandidates =
        new List<GeneratedPlatform>();

    List<GeneratedPlatform>
        mainPathCandidates =
        new List<GeneratedPlatform>();

    foreach (
        GeneratedPlatform platform
        in graph.Platforms)
    {
      if (platform == null)
        continue;

      if (platform.IsStart)
        continue;

      if (platform.IsExit)
        continue;

      if (platform.RouteType ==
          PlatformRouteType.DeadEnd)
      {
        continue;
      }

      if (platform.RouteType ==
          PlatformRouteType.Branch)
      {
        branchCandidates.Add(
            platform
        );

        continue;
      }

      mainPathCandidates.Add(
          platform
      );
    }

    Shuffle(
        branchCandidates,
        random
    );

    Shuffle(
        mainPathCandidates,
        random
    );

    List<GeneratedPlatform>
        candidates =
        new List<GeneratedPlatform>();

    if (preferBranches &&
        branchSelectionWeight > 0f)
    {
      AddWeightedCandidates(
          candidates,
          branchCandidates,
          mainPathCandidates,
          branchSelectionWeight,
          random
      );
    }
    else
    {
      candidates.AddRange(
          mainPathCandidates
      );

      candidates.AddRange(
          branchCandidates
      );
    }

    return candidates;
  }

  private void AddWeightedCandidates(
      List<GeneratedPlatform> result,
      List<GeneratedPlatform> branches,
      List<GeneratedPlatform> mainPath,
      float branchWeight,
      System.Random random
  )
  {
    List<GeneratedPlatform>
        branchPool =
        new List<GeneratedPlatform>(
            branches
        );

    List<GeneratedPlatform>
        mainPool =
        new List<GeneratedPlatform>(
            mainPath
        );

    while (
        branchPool.Count > 0 ||
        mainPool.Count > 0)
    {
      bool chooseBranch;

      if (branchPool.Count == 0)
      {
        chooseBranch = false;
      }
      else if (mainPool.Count == 0)
      {
        chooseBranch = true;
      }
      else
      {
        float totalWeight =
            branchWeight + 1f;

        float roll =
            (float)random.NextDouble()
            * totalWeight;

        chooseBranch =
            roll < branchWeight;
      }

      if (chooseBranch)
      {
        int index =
            random.Next(
                branchPool.Count
            );

        result.Add(
            branchPool[index]
        );

        branchPool.RemoveAt(
            index
        );
      }
      else
      {
        int index =
            random.Next(
                mainPool.Count
            );

        result.Add(
            mainPool[index]
        );

        mainPool.RemoveAt(
            index
        );
      }
    }
  }

  private Vector2 CalculatePosition(
      GeneratedPlatform platform
  )
  {
    return new Vector2(
        platform.Position.x,
        platform.Bounds.yMax +
        verticalOffset
    );
  }

  private List<RelicData>
      GetAvailableRelics()
  {
    List<RelicData>
        result =
        new List<RelicData>();

    if (database == null)
      return result;

    foreach (
        RelicData relic
        in database.Relics)
    {
      if (relic == null)
        continue;

      result.Add(
          relic
      );
    }

    return result;
  }

  private bool ValidateInput(
      PlatformGraph graph,
      Transform parent,
      System.Random random,
      int minimumRelics,
      int maximumRelics
  )
  {
    if (graph == null)
      return false;

    if (parent == null)
      return false;

    if (random == null)
      return false;

    if (relicPickupPrefab == null)
      return false;

    if (database == null)
      return false;

    if (minimumRelics < 0)
      return false;

    if (maximumRelics < minimumRelics)
      return false;

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
        i--)
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
}