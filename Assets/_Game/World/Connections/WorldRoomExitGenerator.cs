using UnityEngine;

public class WorldRoomExitGenerator
{
  private readonly GameObject exitPrefab;

  private readonly float
      horizontalOffset;

  private readonly float
      verticalOffset;

  public WorldRoomExitGenerator(
      GameObject exitPrefab,
      float horizontalOffset,
      float verticalOffset
  )
  {
    this.exitPrefab =
        exitPrefab;

    this.horizontalOffset =
        horizontalOffset;

    this.verticalOffset =
        verticalOffset;
  }

  public void Generate(
      GeneratedWorld world
  )
  {
    if (world == null)
      return;

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

      if (generatedRoom.Markers == null)
        continue;

      GeneratedRoomMarker exitMarker =
          generatedRoom.Markers.RoomExit;

      if (exitMarker == null)
        continue;

      foreach (
          WorldRoom target
          in room.Connections)
      {
        if (target == null)
          continue;

        GeneratedRoom targetRoom =
            world.GetRoom(
                target
            );

        if (targetRoom == null)
          continue;

        GeneratedPlatform
            platform =
            exitMarker.Platform;

        if (platform == null)
          continue;

        RoomExitGenerator generator =
            new RoomExitGenerator(
                exitPrefab,
                horizontalOffset,
                verticalOffset
            );

        GeneratedRoomExit exit =
            generator.Generate(
                room,
                target,
                platform,
                generatedRoom.Instance.transform
            );

        generatedRoom.Exits.Add(
            exit
        );
      }
    }
  }
}