public class PlatformConnection
{
  public GeneratedPlatform From { get; }

  public GeneratedPlatform To { get; }

  public ReachabilityType Type { get; }

  public PlatformConnection(
      GeneratedPlatform from,
      GeneratedPlatform to,
      ReachabilityType type
  )
  {
    From =
        from;

    To =
        to;

    Type =
        type;
  }
}