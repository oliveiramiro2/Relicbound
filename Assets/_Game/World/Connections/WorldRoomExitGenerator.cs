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
        Mathf.Max(
            0f,
            horizontalOffset
        );

    this.verticalOffset =
        Mathf.Max(
            0f,
            verticalOffset
        );
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

      GeneratedPlatform
          exitPlatform =
          generatedRoom.Markers.ExitPlatform;

      if (exitPlatform == null)
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
                exitPlatform,
                generatedRoom.Instance.transform
            );

        generatedRoom.Exits.Add(
            exit
        );
      }
    }
  }
}