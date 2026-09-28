public class HighJumpRelic : Relic
{
    public HighJumpRelic()
        : base("relic.high_jump")
    {
    }

    public override void Apply(RelicContext context)
    {
        context.CapabilityController.AddCapability(
            new JumpHeightCapability(1.25f)
        );
    }
}