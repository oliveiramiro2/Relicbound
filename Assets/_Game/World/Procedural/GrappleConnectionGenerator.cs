using System;
using System.Collections.Generic;
using UnityEngine;

public class GrappleConnectionGenerator
{
    private readonly GameObject grapplePointPrefab;

    private readonly float minimumConnectionDistance;
    private readonly float maximumConnectionDistance;

    private readonly int maximumConnections;
    private readonly int maximumConnectionsPerPlatform;

    private readonly float minimumDistanceFromPlatform;
    private readonly float preferredDistanceFromPlatform;
    private readonly float maximumDistanceFromPlatform;

    private readonly float midpointInfluence;
    private readonly float verticalOffset;

    private readonly float verticalMovementWeight;
    private readonly float horizontalMovementWeight;
    private readonly float distanceWeight;
    private readonly float routeDifferenceWeight;

    public GrappleConnectionGenerator(
        GameObject grapplePointPrefab,
        float minimumConnectionDistance,
        float maximumConnectionDistance,
        int maximumConnections,
        int maximumConnectionsPerPlatform,
        float minimumDistanceFromPlatform,
        float preferredDistanceFromPlatform,
        float maximumDistanceFromPlatform,
        float midpointInfluence,
        float verticalOffset,
        float verticalMovementWeight,
        float horizontalMovementWeight,
        float distanceWeight,
        float routeDifferenceWeight
    )
    {
        this.grapplePointPrefab =
            grapplePointPrefab;

        this.minimumConnectionDistance =
            Mathf.Max(
                0f,
                minimumConnectionDistance
            );

        this.maximumConnectionDistance =
            Mathf.Max(
                this.minimumConnectionDistance,
                maximumConnectionDistance
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

        this.minimumDistanceFromPlatform =
            Mathf.Max(
                0f,
                minimumDistanceFromPlatform
            );

        this.preferredDistanceFromPlatform =
            Mathf.Max(
                this.minimumDistanceFromPlatform,
                preferredDistanceFromPlatform
            );

        this.maximumDistanceFromPlatform =
            Mathf.Max(
                this.preferredDistanceFromPlatform,
                maximumDistanceFromPlatform
            );

        this.midpointInfluence =
            Mathf.Clamp01(
                midpointInfluence
            );

        this.verticalOffset =
            verticalOffset;

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

        this.routeDifferenceWeight =
            Mathf.Max(
                0f,
                routeDifferenceWeight
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
            FindCandidates(
                graph
            );

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

            if (!GrapplePointPlacement.IsValid(
                    candidate.GrapplePosition,
                    candidate.From,
                    candidate.To,
                    minimumDistanceFromPlatform,
                    maximumDistanceFromPlatform
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
        /*
         * UM único grapple point representa
         * a oportunidade de conexão entre
         * as duas plataformas.
         */
        UnityEngine.Object.Instantiate(
            grapplePointPrefab,
            candidate.GrapplePosition,
            Quaternion.identity,
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
                    minimumConnectionDistance)
                {
                    continue;
                }

                if (distance >
                    maximumConnectionDistance)
                {
                    continue;
                }

                Vector2 grapplePosition =
                    GrapplePointPlacement.Calculate(
                        first,
                        second,
                        minimumDistanceFromPlatform,
                        preferredDistanceFromPlatform,
                        maximumDistanceFromPlatform,
                        midpointInfluence,
                        verticalOffset
                    );

                if (!GrapplePointPlacement.IsValid(
                        grapplePosition,
                        first,
                        second,
                        minimumDistanceFromPlatform,
                        maximumDistanceFromPlatform
                    ))
                {
                    continue;
                }

                GrappleCandidate candidate =
                    new GrappleCandidate(
                        first,
                        second,
                        distance,
                        grapplePosition
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

        float verticalRatio =
            absoluteY /
            total;

        float horizontalRatio =
            absoluteX /
            total;

        float distanceRange =
            maximumConnectionDistance -
            minimumConnectionDistance;

        float normalizedDistance =
            distanceRange <= 0.001f
                ? 0.5f
                : Mathf.InverseLerp(
                    minimumConnectionDistance,
                    maximumConnectionDistance,
                    candidate.Distance
                );

        /*
         * O meio da faixa é melhor.
         *
         * Não queremos:
         *
         * - conexões triviais;
         * - conexões absurdamente longas.
         */
        float distanceScore =
            1f -
            Mathf.Abs(
                normalizedDistance -
                0.55f
            );

        /*
         * Quanto mais distante o grapple está
         * das plataformas, mais ele funciona como
         * um verdadeiro ponto de navegação.
         */
        float pointDistanceFromPlatforms =
            Mathf.Min(
                DistanceFromPlatform(
                    candidate.GrapplePosition,
                    candidate.From
                ),
                DistanceFromPlatform(
                    candidate.GrapplePosition,
                    candidate.To
                )
            );

        float pointSpaceScore =
            Mathf.InverseLerp(
                minimumDistanceFromPlatform,
                maximumDistanceFromPlatform,
                pointDistanceFromPlatforms
            );

        /*
         * Penaliza conexões entre plataformas
         * que já estão praticamente lado a lado.
         */
        float routeDifferenceScore =
            Mathf.Clamp01(
                candidate.Distance /
                maximumConnectionDistance
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
            routeDifferenceScore *
            routeDifferenceWeight;

        /*
         * O espaço ao redor do gancho também
         * participa da qualidade.
         */
        score +=
            pointSpaceScore *
            0.35f;

        return score;
    }

    private float DistanceFromPlatform(
        Vector2 position,
        GeneratedPlatform platform
    )
    {
        Vector2 closest =
            new Vector2(
                Mathf.Clamp(
                    position.x,
                    platform.Bounds.min.x,
                    platform.Bounds.max.x
                ),
                Mathf.Clamp(
                    position.y,
                    platform.Bounds.min.y,
                    platform.Bounds.max.y
                )
            );

        return Vector2.Distance(
            position,
            closest
        );
    }

    private int CompareCandidates(
        GrappleCandidate first,
        GrappleCandidate second
    )
    {
        int comparison =
            second.Score.CompareTo(
                first.Score
            );

        if (comparison != 0)
            return comparison;

        comparison =
            first.From.Index.CompareTo(
                second.From.Index
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

        if (maximumConnectionDistance <= 0f)
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

        public Vector2 GrapplePosition { get; }

        public float Score { get; set; }

        public GrappleCandidate(
            GeneratedPlatform from,
            GeneratedPlatform to,
            float distance,
            Vector2 grapplePosition
        )
        {
            From = from;
            To = to;
            Distance = distance;
            GrapplePosition = grapplePosition;
        }
    }
}