public class JumpHeightCapability : PlayerCapability
{
    public float Multiplier { get; }

    public JumpHeightCapability(float multiplier)
    {
        Multiplier = multiplier;
    }
}