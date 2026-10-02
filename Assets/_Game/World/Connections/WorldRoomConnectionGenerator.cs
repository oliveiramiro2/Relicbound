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

      RoomMarkerGenerationResult
          markers =
          generatedRoom.Markers;

      if (markers == null)
        continue;

      GeneratedRoomMarker exit =
          markers.RoomExit;

      if (exit == null)
        continue;

      foreach (
          WorldRoom connectedRoom
          in room.Connections)
      {
        if (connectedRoom == null)
          continue;

        GeneratedRoom targetRoom =
            world.GetRoom(
                connectedRoom
            );

        if (targetRoom == null)
          continue;

        RoomMarkerGenerationResult
            targetMarkers =
            targetRoom.Markers;

        if (targetMarkers == null)
          continue;

        GeneratedRoomMarker entry =
            targetMarkers.RoomEntry;

        if (entry == null)
          continue;

        if (room.Id >=
            connectedRoom.Id)
        {
          continue;
        }

        RoomConnection connection =
            new RoomConnection(
                room,
                connectedRoom,
                exit.Position,
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