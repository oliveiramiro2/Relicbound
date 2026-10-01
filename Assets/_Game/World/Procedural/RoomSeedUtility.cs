public static class RoomSeedUtility
{
  public static int CreateSeed(
      int worldSeed,
      int roomId
  )
  {
    unchecked
    {
      return
          worldSeed * 73856093 +
          roomId * 19349663;
    }
  }
}