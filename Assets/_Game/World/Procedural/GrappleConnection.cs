public class GrappleConnection
{
  public GeneratedPlatform From { get; }

  public GeneratedPlatform To { get; }

  public float Distance { get; }

  public GrappleConnection(
      GeneratedPlatform from,
      GeneratedPlatform to,
      float distance
  )
  {
    From = from;
    To = to;
    Distance = distance;
  }
}