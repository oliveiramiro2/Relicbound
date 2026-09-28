public class DoubleJumpRelic : Relic
{
    private DoubleJumpCapability capability;

    public DoubleJumpRelic()
        : base("relic.double_jump")
    {
    }

    public override void Apply(RelicContext context)
    {
        capability = new DoubleJumpCapability();

        context.CapabilityController.AddCapability(
            capability
        );
    }

    public override void Remove(RelicContext context)
    {
        context.CapabilityController.RemoveCapability(
            capability
        );

        capability = null;
    }
}