using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Movement Reach Settings",
    fileName = "MovementReachSettings"
)]
public class MovementReachSettings : ScriptableObject
{
  [Header("Base Movement")]
  [SerializeField]
  private float maximumHorizontalDistance = 5f;

  [SerializeField]
  private float maximumVerticalUpDistance = 2.5f;

  [SerializeField]
  private float maximumVerticalDownDistance = 2f;

  [Header("Dash")]
  [SerializeField]
  private float dashHorizontalBonus = 2.5f;

  [SerializeField]
  private float dashVerticalBonus = 1.5f;

  [Header("Grapple")]
  [SerializeField]
  private float grappleDistance = 8f;

  public float MaximumHorizontalDistance =>
      maximumHorizontalDistance;

  public float MaximumVerticalUpDistance =>
      maximumVerticalUpDistance;

  public float MaximumVerticalDownDistance =>
      maximumVerticalDownDistance;

  public float DashHorizontalBonus =>
      dashHorizontalBonus;

  public float DashVerticalBonus =>
      dashVerticalBonus;

  public float GrappleDistance =>
      grappleDistance;

  public MovementReachProfile CreateDefaultProfile()
  {
    return new MovementReachProfile(
        maximumHorizontalDistance,
        maximumVerticalUpDistance,
        maximumVerticalDownDistance,
        false,
        true,
        dashHorizontalBonus,
        dashVerticalBonus,
        false,
        grappleDistance
    );
  }
}