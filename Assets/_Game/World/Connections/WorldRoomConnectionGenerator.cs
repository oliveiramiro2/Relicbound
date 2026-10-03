using System.Collections.Generic;

public class WorldRoomConnectionGenerator
{
  public RoomConnectionResult Generate(
      GeneratedWorld world
  )
  {
    List<RoomConnection>
        connections =
        new List<RoomConnection>();

    if (world == null)
    {
      return new RoomConnectionResult(
          connections
      );
    }

    foreach (
        GeneratedRoom generatedRoom
        in world.Rooms)
    {
      if (generatedRoom == null)
        continue;

      WorldRoom room =
          generatedRoom.Room;

      if (room == null)
        continue;

      foreach (
          WorldRoom connectedRoom
          in room.Connections)
      {
        if (connectedRoom == null)
          continue;

        if (room.Id >=
            connectedRoom.Id)
        {
          continue;
        }

        GeneratedRoom targetRoom =
            world.GetRoom(
                connectedRoom
            );

        if (targetRoom == null)
          continue;

        GeneratedRoomExit
            generatedExit =
            generatedRoom.Exits.Find(
                connectedRoom
            );

        if (generatedExit == null)
          continue;

        GeneratedRoomMarker
            entry =
            targetRoom.Markers.RoomEntry;

        if (entry == null)
          continue;

        RoomConnection connection =
            new RoomConnection(
                room,
                connectedRoom,
                generatedExit.Position,
                entry.Position
            );

        connections.Add(
            connection
        );
      }
    }

    return new RoomConnectionResult(
        connections
    );
  }
}