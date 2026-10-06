using System.Collections.Generic;
using UnityEngine;

public class WorldLayoutGenerator
{
    private const float RoomGap = 20f;

    private const float BranchVerticalGap = 20f;

    private readonly WorldRoomTemplateDatabase
        templateDatabase;

    private readonly WorldRoomTemplateSelector
        templateSelector;

    public WorldLayoutGenerator(
        WorldRoomTemplateDatabase templateDatabase
    )
    {
        this.templateDatabase =
            templateDatabase;

        templateSelector =
            new WorldRoomTemplateSelector(
                templateDatabase
            );
    }

    public WorldLayout Generate(
        WorldRoomGraph graph,
        int seed
    )
    {
        WorldLayout layout =
            new WorldLayout();

        if (graph == null)
            return layout;

        if (templateDatabase == null)
            return layout;

        System.Random random =
            new System.Random(
                seed
            );

        Dictionary<WorldRoom, WorldRoomTemplate>
            templates =
            SelectTemplates(
                graph,
                random
            );

        Dictionary<WorldRoom, Vector2>
            positions =
            new Dictionary<
                WorldRoom,
                Vector2
            >();

        LayoutMainPath(
            graph,
            templates,
            positions
        );

        LayoutBranches(
            graph,
            templates,
            positions
        );

        foreach (
            WorldRoom room
            in graph.Rooms)
        {
            if (!templates.TryGetValue(
                    room,
                    out WorldRoomTemplate template))
            {
                continue;
            }

            if (!positions.TryGetValue(
                    room,
                    out Vector2 position))
            {
                continue;
            }

            WorldRoomLayout roomLayout =
                new WorldRoomLayout(
                    room,
                    template,
                    position,
                    template.Size
                );

            layout.AddRoom(
                roomLayout
            );
        }

        return layout;
    }

    private Dictionary<
        WorldRoom,
        WorldRoomTemplate
    > SelectTemplates(
        WorldRoomGraph graph,
        System.Random random
    )
    {
        Dictionary<
            WorldRoom,
            WorldRoomTemplate
        > result =
            new Dictionary<
                WorldRoom,
                WorldRoomTemplate
            >();

        foreach (
            WorldRoom room
            in graph.Rooms)
        {
            WorldRoomTemplate template =
                templateSelector.Select(
                    room,
                    random
                );

            if (template == null)
                continue;

            result.Add(
                room,
                template
            );
        }

        return result;
    }

    private void LayoutMainPath(
        WorldRoomGraph graph,
        Dictionary<
            WorldRoom,
            WorldRoomTemplate
        > templates,
        Dictionary<
            WorldRoom,
            Vector2
        > positions
    )
    {
        WorldRoom previousRoom = null;

        foreach (
            WorldRoom room
            in graph.Rooms)
        {
            if (!templates.TryGetValue(
                    room,
                    out WorldRoomTemplate template))
            {
                continue;
            }

            if (room.Type ==
                WorldRoomType.Branch)
            {
                continue;
            }

            if (previousRoom == null)
            {
                positions[room] =
                    Vector2.zero;

                previousRoom =
                    room;

                continue;
            }

            if (!templates.TryGetValue(
                    previousRoom,
                    out WorldRoomTemplate previousTemplate))
            {
                continue;
            }

            Vector2 previousPosition =
                positions[previousRoom];

            float previousRight =
                previousPosition.x +
                previousTemplate.Size.x / 2f;

            float currentLeft =
                template.Size.x / 2f;

            float x =
                previousRight +
                RoomGap +
                currentLeft;

            float y =
                previousPosition.y;

            positions[room] =
                new Vector2(
                    x,
                    y
                );

            previousRoom =
                room;
        }
    }

    private void LayoutBranches(
        WorldRoomGraph graph,
        Dictionary<
            WorldRoom,
            WorldRoomTemplate
        > templates,
        Dictionary<
            WorldRoom,
            Vector2
        > positions
    )
    {
        foreach (
            WorldRoom room
            in graph.Rooms)
        {
            if (room.Type !=
                WorldRoomType.Branch)
            {
                continue;
            }

            if (!templates.TryGetValue(
                    room,
                    out WorldRoomTemplate template))
            {
                continue;
            }

            WorldRoom parent =
                FindLayoutParent(
                    room,
                    positions
                );

            if (parent == null)
                continue;

            if (!templates.TryGetValue(
                    parent,
                    out WorldRoomTemplate parentTemplate))
            {
                continue;
            }

            if (!positions.TryGetValue(
                    parent,
                    out Vector2 parentPosition))
            {
                continue;
            }

            float parentTop =
                parentPosition.y +
                parentTemplate.Size.y / 2f;

            float branchBottom =
                template.Size.y / 2f;

            float y =
                parentTop +
                BranchVerticalGap +
                branchBottom;

            float x =
                parentPosition.x;

            positions[room] =
                new Vector2(
                    x,
                    y
                );
        }
    }

    private WorldRoom FindLayoutParent(
        WorldRoom room,
        Dictionary<
            WorldRoom,
            Vector2
        > positions
    )
    {
        foreach (
            WorldRoom connectedRoom
            in room.Connections)
        {
            if (positions.ContainsKey(
                    connectedRoom))
            {
                return connectedRoom;
            }
        }

        return null;
    }
}