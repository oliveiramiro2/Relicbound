using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldLayoutGenerator
{
    private const float RoomGap = 2f;
    private const float BranchVerticalGap = 2f;

    private readonly WorldRoomTemplateSelector templateSelector;

    public WorldLayoutGenerator(
        WorldRoomTemplateDatabase database
    )
    {
        templateSelector =
            new WorldRoomTemplateSelector(
                database
            );
    }

    public WorldLayout Generate(
        WorldRoomGraph graph,
        int seed
    )
    {
        WorldLayout layout = new();

        Dictionary<WorldRoom, WorldRoomLayout> layouts = new();

        Dictionary<WorldRoom, int> branchCounts = new();

        System.Random random = new(seed);

        foreach (WorldRoom room in graph.Rooms)
        {
            WorldRoomTemplate template =
                templateSelector.Select(
                    room,
                    random
                );

            Vector2 position =
                CalculatePosition(
                    room,
                    layouts,
                    branchCounts,
                    template
                );

            Vector2 size =
                template != null
                    ? template.Size
                    : Vector2.one;

            WorldRoomLayout roomLayout =
                new WorldRoomLayout(
                    room,
                    template,
                    position,
                    size
                );

            layout.AddRoom(
                roomLayout
            );

            layouts.Add(
                room,
                roomLayout
            );
        }

        return layout;
    }

    private Vector2 CalculatePosition(
        WorldRoom room,
        Dictionary<WorldRoom, WorldRoomLayout> layouts,
        Dictionary<WorldRoom, int> branchCounts,
        WorldRoomTemplate template
    )
    {
        if (room.Type == WorldRoomType.Start)
        {
            return Vector2.zero;
        }

        WorldRoom parent =
            FindParent(room);

        if (parent == null)
        {
            return Vector2.zero;
        }

        if (!layouts.TryGetValue(
                parent,
                out WorldRoomLayout parentLayout))
        {
            return Vector2.zero;
        }

        Vector2 parentPosition =
            parentLayout.Position;

        Vector2 parentSize =
            parentLayout.Size;

        Vector2 currentSize =
            template != null
                ? template.Size
                : Vector2.one;

        if (room.Type == WorldRoomType.Branch)
        {
            int branchIndex = 0;

            if (branchCounts.TryGetValue(
                    parent,
                    out int count))
            {
                branchIndex = count;
            }

            branchCounts[parent] =
                branchIndex + 1;

            float direction =
                branchIndex % 2 == 0
                    ? 1f
                    : -1f;

            float verticalDistance =
                parentSize.y / 2f +
                currentSize.y / 2f +
                BranchVerticalGap;

            return parentPosition +
                new Vector2(
                    0f,
                    direction * verticalDistance
                );
        }

        float horizontalDistance =
            parentSize.x / 2f +
            currentSize.x / 2f +
            RoomGap;

        return parentPosition +
            new Vector2(
                horizontalDistance,
                0f
            );
    }

    private WorldRoom FindParent(
        WorldRoom room
    )
    {
        foreach (WorldRoom connection in room.Connections)
        {
            if (connection.Id < room.Id)
                return connection;
        }

        return null;
    }
}