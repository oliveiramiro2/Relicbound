public class SpeedRelic : Relic
{
    public SpeedRelic()
        : base("relic.speed")
    {
    }

    public override void Apply(RelicContext context)
    {
        context.CapabilityController.AddCapability(
            new MovementSpeedCapability(1.25f)
        );
    }
}