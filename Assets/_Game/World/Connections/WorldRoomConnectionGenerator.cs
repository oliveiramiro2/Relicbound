using System.Collections.Generic;
using UnityEngine;

public class WorldRoomConnectionGenerator
{
  public RoomConnectionResult Generate(GeneratedWorld world)
  {
    if (world == null)
      return new RoomConnectionResult(
          new List<RoomConnection>());

    List<RoomConnection> connections =
        new List<RoomConnection>();

    foreach (GeneratedRoom generatedRoom in world.Rooms)
    {
      if (generatedRoom == null)
        continue;

      WorldRoom room = generatedRoom.Room;

      if (room == null)
        continue;

      foreach (WorldRoom target in room.Connections)
      {
        if (target == null)
          continue;

        GeneratedRoom targetRoom =
            world.GetRoom(target);

        if (targetRoom == null)
          continue;

        GeneratedRoomExit exit =
            FindExit(
                generatedRoom,
                target);

        if (exit == null)
          continue;

        Vector2 direction =
            targetRoom.Instance.transform.position -
            generatedRoom.Instance.transform.position;

        Vector2 entryPosition =
            RoomEntryPositionCalculator.Calculate(
                targetRoom,
                -direction);

        RoomConnection connection =
            new RoomConnection(
                room,
                target,
                exit.Position,
                entryPosition);

        connections.Add(connection);
      }
    }

    return new RoomConnectionResult(
        connections);
  }

  private GeneratedRoomExit FindExit(
      GeneratedRoom room,
      WorldRoom target)
  {
    if (room == null || target == null)
      return null;

    return room.Exits.Find(target);
  }
}