public class MovementSpeedCapability : PlayerCapability
{
    public float Multiplier { get; }

    public MovementSpeedCapability(float multiplier)
    {
        Multiplier = multiplier;
    }
}