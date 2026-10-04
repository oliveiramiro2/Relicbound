using UnityEngine;

[CreateAssetMenu(
    menuName = "World/Procedural Room Settings",
    fileName = "ProceduralRoomSettings"
)]
public class ProceduralRoomSettings : ScriptableObject
{
    [Header("Main Path")]

    [SerializeField]
    private int platformCount = 5;

    [SerializeField]
    [Range(0.1f, 0.45f)]
    private float startHorizontalPosition = 0.15f;

    [SerializeField]
    [Range(0.55f, 0.95f)]
    private float exitHorizontalPosition = 0.85f;

    [SerializeField]
    [Range(0f, 1f)]
    private float mainPathVerticalVariation = 0.35f;

    [Header("Branches")]

    [SerializeField]
    private int minimumBranches = 1;

    [SerializeField]
    private int maximumBranches = 2;

    [SerializeField]
    private int minimumBranchLength = 1;

    [SerializeField]
    private int maximumBranchLength = 3;

    [Header("Platform Spacing")]

    [SerializeField]
    private float minimumPlatformSpacing = 1f;

    [Header("Generation")]

    [SerializeField]
    private int maximumAttemptsPerPlatform = 30;

    public int PlatformCount =>
        platformCount;

    public float StartHorizontalPosition =>
        Mathf.Clamp(
            startHorizontalPosition,
            0.1f,
            0.45f
        );

    public float ExitHorizontalPosition =>
        Mathf.Clamp(
            exitHorizontalPosition,
            0.55f,
            0.95f
        );

    public float MainPathVerticalVariation =>
        Mathf.Clamp01(
            mainPathVerticalVariation
        );

    public int MinimumBranches =>
        minimumBranches;

    public int MaximumBranches =>
        maximumBranches;

    public int MinimumBranchLength =>
        minimumBranchLength;

    public int MaximumBranchLength =>
        maximumBranchLength;

    public float MinimumPlatformSpacing =>
        minimumPlatformSpacing;

    public int MaximumAttemptsPerPlatform =>
        maximumAttemptsPerPlatform;
}