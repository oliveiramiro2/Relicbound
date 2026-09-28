public class HighJumpRelic : Relic
{
    private JumpHeightCapability capability;

    public HighJumpRelic()
        : base("relic.high_jump")
    {
    }

    public override void Apply(RelicContext context)
    {
        capability = new JumpHeightCapability(1.25f);

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