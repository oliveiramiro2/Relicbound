using System.Collections.Generic;

public class GeneratedRoomExits
{
  private readonly List<GeneratedRoomExit>
      exits =
      new();

  public IReadOnlyList<GeneratedRoomExit>
      Exits =>
      exits;

  public int Count =>
      exits.Count;

  public void Add(
      GeneratedRoomExit exit
  )
  {
    if (exit == null)
      return;

    exits.Add(
        exit
    );
  }

  public GeneratedRoomExit Find(
      WorldRoom target
  )
  {
    if (target == null)
      return null;

    foreach (
        GeneratedRoomExit exit
        in exits)
    {
      if (exit.To == target)
        return exit;
    }

    return null;
  }
}