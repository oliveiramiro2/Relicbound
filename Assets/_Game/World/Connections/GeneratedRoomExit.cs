using UnityEngine;

public class GeneratedRoomExit
{
    public WorldRoom From { get; }

    public WorldRoom To { get; }

    public Vector2 Position { get; }

    public Vector2 Direction { get; }

    public GeneratedRoomExit(
        WorldRoom from,
        WorldRoom to,
        Vector2 position,
        Vector2 direction
    )
    {
        From = from;
        To = to;
        Position = position;
        Direction = direction;
    }
}