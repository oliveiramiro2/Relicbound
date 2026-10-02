using System;
using System.Collections.Generic;
using UnityEngine;

public class GrappleConnectionGenerator
{
  private readonly GameObject grapplePointPrefab;

  private readonly float grappleDistance;

  private readonly int maximumConnections;

  private readonly float distanceFromPlatform;

  public GrappleConnectionGenerator(
      GameObject grapplePointPrefab,
      float grappleDistance,
      int maximumConnections,
      float distanceFromPlatform
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

    this.distanceFromPlatform =
        Mathf.Max(
            0f,
            distanceFromPlatform
        );
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

    if (!ValidateInput(
            graph,
            parent,
            random))
    {
      return new GrappleConnectionResult(
          connections
      );
    }

    List<GrappleCandidate>
        candidates =
        FindCandidates(
            graph
        );

    Shuffle(
        candidates,
        random
    );

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

      CreateGrapplePoint(
          candidate.From,
          candidate.To,
          parent
      );

      CreateGrapplePoint(
          candidate.To,
          candidate.From,
          parent
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

  private void CreateGrapplePoint(
      GeneratedPlatform platform,
      GeneratedPlatform target,
      Transform parent
  )
  {
    Vector2 position =
        GrapplePointPlacement.Calculate(
            platform,
            target,
            distanceFromPlatform
        );

    UnityEngine.Object.Instantiate(
        grapplePointPrefab,
        position,
        Quaternion.identity,
        parent
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

  private bool ValidateInput(
      PlatformGraph graph,
      Transform parent,
      System.Random random
  )
  {
    if (graph == null)
      return false;

    if (parent == null)
      return false;

    if (random == null)
      return false;

    if (grapplePointPrefab == null)
      return false;

    if (grappleDistance <= 0f)
      return false;

    if (maximumConnections <= 0)
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