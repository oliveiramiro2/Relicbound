using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardGenerator
{
    private readonly ProceduralRewardSettings settings;

    public RewardGenerator(
        ProceduralRewardSettings settings
    )
    {
        this.settings = settings;
    }

    public RewardGenerationResult Generate(
        PlatformGenerationResult platformResult,
        int seed
    )
    {
        RewardGenerationResult result =
            new RewardGenerationResult();

        if (settings == null)
            return result;

        if (!settings.Enabled)
            return result;

        if (platformResult == null)
            return result;

        if (settings.MaximumRewardsPerRoom <= 0)
            return result;

        List<GeneratedPlatform> candidates =
            CreateCandidates(platformResult);

        if (candidates.Count == 0)
            return result;

        System.Random random =
            new System.Random(seed);

        Shuffle(candidates, random);

        int rewardCount =
            Mathf.Min(
                settings.MaximumRewardsPerRoom,
                candidates.Count
            );

        for (int i = 0; i < rewardCount; i++)
        {
            GeneratedPlatform platform =
                candidates[i];

            ProceduralRewardType rewardType =
                SelectRewardType(random);

            Vector2 position =
                CalculatePosition(platform);

            GeneratedReward reward =
                new GeneratedReward(
                    rewardType,
                    position,
                    platform
                );

            result.Add(reward);
        }

        return result;
    }

    private List<GeneratedPlatform> CreateCandidates(
        PlatformGenerationResult platformResult
    )
    {
        List<GeneratedPlatform> candidates =
            new List<GeneratedPlatform>();

        foreach (GeneratedPlatform platform
                 in platformResult.MainPathPlatforms)
        {
            if (platform == null)
                continue;

            if (platform.IsStart)
                continue;

            if (platform.IsExit)
                continue;

            candidates.Add(platform);
        }

        foreach (GeneratedPlatform platform
                 in platformResult.BranchPlatforms)
        {
            if (platform == null)
                continue;

            if (platform.IsStart)
                continue;

            if (platform.IsExit)
                continue;

            candidates.Add(platform);
        }

        return candidates;
    }

    private ProceduralRewardType SelectRewardType(
        System.Random random
    )
    {
        bool canRelic =
            settings.AllowRelics;

        bool canSlotExpansion =
            settings.AllowSlotExpansions;

        if (canRelic && canSlotExpansion)
        {
            return random.Next(0, 2) == 0
                ? ProceduralRewardType.Relic
                : ProceduralRewardType.SlotExpansion;
        }

        if (canRelic)
            return ProceduralRewardType.Relic;

        return ProceduralRewardType.SlotExpansion;
    }

    private Vector2 CalculatePosition(
        GeneratedPlatform platform
    )
    {
        float x =
            platform.Position.x;

        float y =
            platform.Bounds.yMax +
            settings.VerticalOffset;

        return new Vector2(x, y);
    }

    private void Shuffle(
        List<GeneratedPlatform> platforms,
        System.Random random
    )
    {
        for (int i = platforms.Count - 1; i > 0; i--)
        {
            int index =
                random.Next(i + 1);

            GeneratedPlatform temporary =
                platforms[i];

            platforms[i] =
                platforms[index];

            platforms[index] =
                temporary;
        }
    }
}