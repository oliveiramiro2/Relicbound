public class DoubleJumpRelic : Relic
{
    public DoubleJumpRelic()
        : base("relic.double_jump")
    {
    }

    public override void Apply(RelicContext context)
    {
        context.CapabilityController.AddCapability(
            new DoubleJumpCapability()
        );
    }
}