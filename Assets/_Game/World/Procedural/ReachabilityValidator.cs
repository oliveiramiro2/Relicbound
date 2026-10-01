using UnityEngine;

public class ReachabilityValidator
{
  public ReachabilityCheckResult Check(
      GeneratedPlatform from,
      GeneratedPlatform to,
      MovementReachProfile profile
  )
  {
    if (from == null ||
        to == null ||
        profile == null)
    {
      return new ReachabilityCheckResult(
          false,
          ReachabilityType.Unreachable,
          0f,
          0f
      );
    }

    float horizontalDistance =
        Mathf.Abs(
            to.Position.x -
            from.Position.x
        );

    float verticalDifference =
        to.Position.y -
        from.Position.y;

    float verticalDistance =
        Mathf.Abs(
            verticalDifference
        );

    if (horizontalDistance <=
        profile.MaximumHorizontalDistance &&
        IsVerticalReachable(
            verticalDifference,
            profile.MaximumVerticalUpDistance,
            profile.MaximumVerticalDownDistance
        ))
    {
      return new ReachabilityCheckResult(
          true,
          ReachabilityType.NormalMovement,
          horizontalDistance,
          verticalDistance
      );
    }

    if (profile.CanDoubleJump &&
        horizontalDistance <=
        profile.EffectiveHorizontalDistance &&
        IsVerticalReachable(
            verticalDifference,
            profile.EffectiveVerticalUpDistance,
            profile.EffectiveVerticalDownDistance
        ))
    {
      return new ReachabilityCheckResult(
          true,
          ReachabilityType.DoubleJump,
          horizontalDistance,
          verticalDistance
      );
    }

    if (profile.CanDash &&
        horizontalDistance <=
        profile.EffectiveHorizontalDistance &&
        IsVerticalReachable(
            verticalDifference,
            profile.EffectiveVerticalUpDistance,
            profile.EffectiveVerticalDownDistance
        ))
    {
      return new ReachabilityCheckResult(
          true,
          ReachabilityType.Dash,
          horizontalDistance,
          verticalDistance
      );
    }

    if (profile.CanGrapple &&
        horizontalDistance <=
        profile.GrappleDistance &&
        verticalDistance <=
        profile.GrappleDistance)
    {
      float distance =
          Vector2.Distance(
              from.Position,
              to.Position
          );

      if (distance <=
          profile.GrappleDistance)
      {
        return new ReachabilityCheckResult(
            true,
            ReachabilityType.Grapple,
            horizontalDistance,
            verticalDistance
        );
      }
    }

    return new ReachabilityCheckResult(
        false,
        ReachabilityType.Unreachable,
        horizontalDistance,
        verticalDistance
    );
  }

  private bool IsVerticalReachable(
      float verticalDifference,
      float maximumUpDistance,
      float maximumDownDistance
  )
  {
    if (verticalDifference > 0f)
    {
      return verticalDifference <=
          maximumUpDistance;
    }

    return Mathf.Abs(
        verticalDifference
    ) <= maximumDownDistance;
  }
}