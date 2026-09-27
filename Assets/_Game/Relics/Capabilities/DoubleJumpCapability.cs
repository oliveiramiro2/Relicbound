public class DoubleJumpCapability : PlayerCapability
{
    public int ExtraJumps { get; }

    public DoubleJumpCapability(int extraJumps = 1)
    {
        ExtraJumps = extraJumps;
    }
}