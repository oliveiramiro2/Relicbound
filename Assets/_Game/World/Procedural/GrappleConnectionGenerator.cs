using System;
using System.Collections.Generic;
using UnityEngine;

public class GrappleConnectionGenerator
{
    private readonly GameObject grapplePointPrefab;

    private readonly float minimumGrappleDistance;
    private readonly float maximumGrappleDistance;

    private readonly int maximumConnections;
    private readonly int maximumConnectionsPerPlatform;

    private readonly float distanceFromPlatform;

    private readonly float preferredDistance;

    private readonly float verticalMovementWeight;
    private readonly float horizontalMovementWeight;
    private readonly float distanceWeight;
    private readonly float separationWeight;

    public GrappleConnectionGenerator(
        GameObject grapplePointPrefab,
        float minimumGrappleDistance,
        float maximumGrappleDistance,
        int maximumConnections,
        int maximumConnectionsPerPlatform,
        float distanceFromPlatform,
        float preferredDistance,
        float verticalMovementWeight,
        float horizontalMovementWeight,
        float distanceWeight,
        float separationWeight
    )
    {
        this.grapplePointPrefab =
            grapplePointPrefab;

        this.minimumGrappleDistance =
            Mathf.Max(
                0f,
                minimumGrappleDistance
            );

        this.maximumGrappleDistance =
            Mathf.Max(
                this.minimumGrappleDistance,
                maximumGrappleDistance
            );

        this.maximumConnections =
            Mathf.Max(
                0,
                maximumConnections
            );

        this.maximumConnectionsPerPlatform =
            Mathf.Max(
                1,
                maximumConnectionsPerPlatform
            );

        this.distanceFromPlatform =
            Mathf.Max(
                0f,
                distanceFromPlatform
            );

        this.preferredDistance =
            Mathf.Clamp01(
                preferredDistance
            );

        this.verticalMovementWeight =
            Mathf.Max(
                0f,
                verticalMovementWeight
            );

        this.horizontalMovementWeight =
            Mathf.Max(
                0f,
                horizontalMovementWeight
            );

        this.distanceWeight =
            Mathf.Max(
                0f,
                distanceWeight
            );

        this.separationWeight =
            Mathf.Max(
                0f,
                separationWeight
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
                random
            ))
        {
            return new GrappleConnectionResult(
                connections
            );
        }

        List<GrappleCandidate>
            candidates =
            FindCandidates(graph);

        EvaluateCandidates(
            candidates
        );

        candidates.Sort(
            CompareCandidates
        );

        Dictionary<GeneratedPlatform, int>
            connectionCounts =
            new Dictionary<GeneratedPlatform, int>();

        foreach (
            GrappleCandidate candidate
            in candidates
        )
        {
            if (connections.Count >=
                maximumConnections)
            {
                break;
            }

            if (HasReachedPlatformLimit(
                    connectionCounts,
                    candidate.From
                ))
            {
                continue;
            }

            if (HasReachedPlatformLimit(
                    connectionCounts,
                    candidate.To
                ))
            {
                continue;
            }

            if (HasExistingConnection(
                    graph,
                    candidate.From,
                    candidate.To
                ))
            {
                continue;
            }

            CreateConnection(
                graph,
                parent,
                candidate,
                connections
            );

            IncrementConnectionCount(
                connectionCounts,
                candidate.From
            );

            IncrementConnectionCount(
                connectionCounts,
                candidate.To
            );
        }

        return new GrappleConnectionResult(
            connections
        );
    }

    private void CreateConnection(
        PlatformGraph graph,
        Transform parent,
        GrappleCandidate candidate,
        List<GrappleConnection> connections
    )
    {
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
            i++
        )
        {
            GeneratedPlatform first =
                platforms[i];

            if (first == null)
                continue;

            for (
                int j = i + 1;
                j < platforms.Count;
                j++
            )
            {
                GeneratedPlatform second =
                    platforms[j];

                if (second == null)
                    continue;

                if (HasExistingConnection(
                        graph,
                        first,
                        second
                    ))
                {
                    continue;
                }

                float distance =
                    Vector2.Distance(
                        first.Position,
                        second.Position
                    );

                if (distance <
                    minimumGrappleDistance)
                {
                    continue;
                }

                if (distance >
                    maximumGrappleDistance)
                {
                    continue;
                }

                GrappleCandidate candidate =
                    new GrappleCandidate(
                        first,
                        second,
                        distance
                    );

                candidates.Add(
                    candidate
                );
            }
        }

        return candidates;
    }

    private void EvaluateCandidates(
        List<GrappleCandidate> candidates
    )
    {
        foreach (
            GrappleCandidate candidate
            in candidates
        )
        {
            candidate.Score =
                CalculateScore(
                    candidate
                );
        }
    }

    private float CalculateScore(
        GrappleCandidate candidate
    )
    {
        Vector2 difference =
            candidate.To.Position -
            candidate.From.Position;

        float absoluteX =
            Mathf.Abs(
                difference.x
            );

        float absoluteY =
            Mathf.Abs(
                difference.y
            );

        float total =
            absoluteX +
            absoluteY;

        if (total <= 0.001f)
            return 0f;

        /*
         * Quanto mais vertical for o deslocamento,
         * maior a utilidade potencial do grapple.
         */
        float verticalRatio =
            absoluteY /
            total;

        /*
         * Quanto mais horizontal for o deslocamento,
         * mais útil para atravessar gaps.
         */
        float horizontalRatio =
            absoluteX /
            total;

        /*
         * Normaliza a distância para 0..1.
         */
        float distanceRange =
            maximumGrappleDistance -
            minimumGrappleDistance;

        float distanceNormalized =
            distanceRange <= 0.001f
                ? 1f
                : Mathf.InverseLerp(
                    minimumGrappleDistance,
                    maximumGrappleDistance,
                    candidate.Distance
                );

        /*
         * Queremos evitar:
         *
         * - grapples extremamente curtos
         * - grapples no limite máximo
         *
         * O pico fica na distância preferida.
         */
        float distanceScore =
            1f -
            Mathf.Abs(
                distanceNormalized -
                preferredDistance
            );

        /*
         * Um pouco de preferência por plataformas
         * separadas verticalmente.
         */
        float separationScore =
            Mathf.Clamp01(
                candidate.Distance /
                Mathf.Max(
                    maximumGrappleDistance,
                    0.001f
                )
            );

        float score =
            verticalRatio *
            verticalMovementWeight;

        score +=
            horizontalRatio *
            horizontalMovementWeight;

        score +=
            distanceScore *
            distanceWeight;

        score +=
            separationScore *
            separationWeight;

        return score;
    }

    private int CompareCandidates(
        GrappleCandidate first,
        GrappleCandidate second
    )
    {
        int scoreComparison =
            second.Score.CompareTo(
                first.Score
            );

        if (scoreComparison != 0)
            return scoreComparison;

        /*
         * Empate determinístico.
         *
         * Isso evita que a geração dependa
         * de detalhes do Sort.
         */
        int firstFromId =
            first.From.Index;

        int secondFromId =
            second.From.Index;

        int comparison =
            firstFromId.CompareTo(
                secondFromId
            );

        if (comparison != 0)
            return comparison;

        return first.To.Index.CompareTo(
            second.To.Index
        );
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
            in connections
        )
        {
            if (connection.To == second)
                return true;
        }

        return false;
    }

    private bool HasReachedPlatformLimit(
        Dictionary<GeneratedPlatform, int>
            connectionCounts,
        GeneratedPlatform platform
    )
    {
        if (!connectionCounts.TryGetValue(
                platform,
                out int count
            ))
        {
            return false;
        }

        return count >=
               maximumConnectionsPerPlatform;
    }

    private void IncrementConnectionCount(
        Dictionary<GeneratedPlatform, int>
            connectionCounts,
        GeneratedPlatform platform
    )
    {
        if (connectionCounts.TryGetValue(
                platform,
                out int count
            ))
        {
            connectionCounts[platform] =
                count + 1;

            return;
        }

        connectionCounts.Add(
            platform,
            1
        );
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

        if (maximumGrappleDistance <= 0f)
            return false;

        if (maximumConnections <= 0)
            return false;

        return true;
    }

    private class GrappleCandidate
    {
        public GeneratedPlatform From { get; }

        public GeneratedPlatform To { get; }

        public float Distance { get; }

        public float Score { get; set; }

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