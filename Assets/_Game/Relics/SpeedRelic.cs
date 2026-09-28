public class SpeedRelic : Relic
{
    public SpeedRelic()
        : base("relic.speed")
    {
    }

    public override void Apply(
        PlayerCapabilityController capabilityController
    )
    {
        capabilityController.AddCapability(
            new MovementSpeedCapability(1.25f)
        );
    }
}