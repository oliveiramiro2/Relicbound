using System;

public class WorldRoomTemplateSelector
{
  private readonly WorldRoomTemplateDatabase database;

  public WorldRoomTemplateSelector(
      WorldRoomTemplateDatabase database
  )
  {
    this.database = database;
  }

  public WorldRoomTemplate Select(
      WorldRoom room,
      Random random
  )
  {
    if (room == null)
      return null;

    if (database == null)
      return null;

    if (random == null)
      return null;

    return database.GetRandomTemplate(
        room.Type,
        random
    );
  }
}