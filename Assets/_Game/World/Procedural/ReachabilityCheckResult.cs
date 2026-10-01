public class ReachabilityCheckResult
{
  public bool IsReachable { get; }

  public ReachabilityType Type { get; }

  public float HorizontalDistance { get; }

  public float VerticalDistance { get; }

  public ReachabilityCheckResult(
      bool isReachable,
      ReachabilityType type,
      float horizontalDistance,
      float verticalDistance
  )
  {
    IsReachable =
        isReachable;

    Type =
        type;

    HorizontalDistance =
        horizontalDistance;

    VerticalDistance =
        verticalDistance;
  }
}