using System;
using System.Collections.Generic;
using UnityEngine;

public class GrappleConnectionGenerator
{
  private readonly GameObject grapplePointPrefab;

  private readonly float grappleDistance;

  private readonly int maximumConnections;

  private readonly float verticalOffset;

  public GrappleConnectionGenerator(
      GameObject grapplePointPrefab,
      float grappleDistance,
      int maximumConnections,
      float verticalOffset
  )
  {
    this.grapplePointPrefab =
        grapplePointPrefab;

    this.grappleDistance =
        Mathf.Max(
            0f,
            grappleDistance
        );

    this.maximumConnections =
        Mathf.Max(
            0,
            maximumConnections
        );

    this.verticalOffset =
        verticalOffset;
  }

  public GrappleConnectionResult Generate(
      PlatformGraph graph,
      Transform parent,
      System.Random random
  )
  {
    List<GrappleConnection>
        connections =
        new List<GrappleConnection>();

    if (graph == null)
      return new GrappleConnectionResult(
          connections
      );

    if (parent == null)
      return new GrappleConnectionResult(
          connections
      );

    if (random == null)
      return new GrappleConnectionResult(
          connections
      );

    if (grapplePointPrefab == null)
      return new GrappleConnectionResult(
          connections
      );

    if (grappleDistance <= 0f)
      return new GrappleConnectionResult(
          connections
      );

    if (maximumConnections <= 0)
      return new GrappleConnectionResult(
          connections
      );

    List<GrappleCandidate>
        candidates =
        FindCandidates(
            graph
        );

    Shuffle(
        candidates,
        random
    );

    HashSet<GeneratedPlatform>
        platformsWithPoints =
        new HashSet<GeneratedPlatform>();

    foreach (
        GrappleCandidate candidate
        in candidates)
    {
      if (connections.Count >=
          maximumConnections)
      {
        break;
      }

      if (HasExistingConnection(
              graph,
              candidate.From,
              candidate.To))
      {
        continue;
      }

      EnsureGrapplePoint(
          candidate.From,
          parent,
          platformsWithPoints
      );

      EnsureGrapplePoint(
          candidate.To,
          parent,
          platformsWithPoints
      );

      graph.Connect(
          candidate.From,
          candidate.To,
          ReachabilityType.Grapple
      );

      GrappleConnection connection =
          new GrappleConnection(
              candidate.From,
              candidate.To,
              candidate.Distance
          );

      connections.Add(
          connection
      );
    }

    return new GrappleConnectionResult(
        connections
    );
  }

  private List<GrappleCandidate>
      FindCandidates(
          PlatformGraph graph
      )
  {
    List<GrappleCandidate>
        candidates =
        new List<GrappleCandidate>();

    IReadOnlyList<GeneratedPlatform>
        platforms =
        graph.Platforms;

    for (
        int i = 0;
        i < platforms.Count;
        i++)
    {
      GeneratedPlatform first =
          platforms[i];

      for (
          int j = i + 1;
          j < platforms.Count;
          j++)
      {
        GeneratedPlatform second =
            platforms[j];

        float distance =
            Vector2.Distance(
                first.Position,
                second.Position
            );

        if (distance >
            grappleDistance)
        {
          continue;
        }

        candidates.Add(
            new GrappleCandidate(
                first,
                second,
                distance
            )
        );
      }
    }

    return candidates;
  }

  private bool HasExistingConnection(
      PlatformGraph graph,
      GeneratedPlatform first,
      GeneratedPlatform second
  )
  {
    IReadOnlyList<PlatformConnection>
        connections =
        graph.GetConnections(
            first
        );

    foreach (
        PlatformConnection connection
        in connections)
    {
      if (connection.To == second)
        return true;
    }

    return false;
  }

  private void EnsureGrapplePoint(
      GeneratedPlatform platform,
      Transform parent,
      HashSet<GeneratedPlatform>
          platformsWithPoints
  )
  {
    if (platform == null)
      return;

    if (platformsWithPoints.Contains(
            platform))
    {
      return;
    }

    Vector2 pointPosition =
        GetGrapplePointPosition(
            platform
        );

    UnityEngine.Object.Instantiate(
        grapplePointPrefab,
        pointPosition,
        Quaternion.identity,
        parent
    );

    platformsWithPoints.Add(
        platform
    );
  }

  private Vector2 GetGrapplePointPosition(
      GeneratedPlatform platform
  )
  {
    return new Vector2(
        platform.Position.x,
        platform.Bounds.yMax +
        verticalOffset
    );
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

  private class GrappleCandidate
  {
    public GeneratedPlatform From { get; }

    public GeneratedPlatform To { get; }

    public float Distance { get; }

    public GrappleCandidate(
        GeneratedPlatform from,
        GeneratedPlatform to,
        float distance
    )
    {
      From = from;

      To = to;

      Distance = distance;
    }
  }
}