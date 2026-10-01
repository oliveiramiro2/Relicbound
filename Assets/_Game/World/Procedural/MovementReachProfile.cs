using UnityEngine;

public class MovementReachProfile
{
  public float MaximumHorizontalDistance { get; }

  public float MaximumVerticalUpDistance { get; }

  public float MaximumVerticalDownDistance { get; }

  public bool CanDoubleJump { get; }

  public bool CanDash { get; }

  public float DashHorizontalBonus { get; }

  public float DashVerticalBonus { get; }

  public bool CanGrapple { get; }

  public float GrappleDistance { get; }

  public MovementReachProfile(
      float maximumHorizontalDistance,
      float maximumVerticalUpDistance,
      float maximumVerticalDownDistance,
      bool canDoubleJump,
      bool canDash,
      float dashHorizontalBonus,
      float dashVerticalBonus,
      bool canGrapple,
      float grappleDistance
  )
  {
    MaximumHorizontalDistance =
        Mathf.Max(
            0f,
            maximumHorizontalDistance
        );

    MaximumVerticalUpDistance =
        Mathf.Max(
            0f,
            maximumVerticalUpDistance
        );

    MaximumVerticalDownDistance =
        Mathf.Max(
            0f,
            maximumVerticalDownDistance
        );

    CanDoubleJump =
        canDoubleJump;

    CanDash =
        canDash;

    DashHorizontalBonus =
        Mathf.Max(
            0f,
            dashHorizontalBonus
        );

    DashVerticalBonus =
        Mathf.Max(
            0f,
            dashVerticalBonus
        );

    CanGrapple =
        canGrapple;

    GrappleDistance =
        Mathf.Max(
            0f,
            grappleDistance
        );
  }

  public float EffectiveHorizontalDistance
  {
    get
    {
      float result =
          MaximumHorizontalDistance;

      if (CanDash)
      {
        result +=
            DashHorizontalBonus;
      }

      return result;
    }
  }

  public float EffectiveVerticalUpDistance
  {
    get
    {
      float result =
          MaximumVerticalUpDistance;

      if (CanDoubleJump)
      {
        result *= 1.35f;
      }

      if (CanDash)
      {
        result +=
            DashVerticalBonus;
      }

      return result;
    }
  }

  public float EffectiveVerticalDownDistance
  {
    get
    {
      float result =
          MaximumVerticalDownDistance;

      if (CanDash)
      {
        result +=
            DashVerticalBonus;
      }

      return result;
    }
  }
}