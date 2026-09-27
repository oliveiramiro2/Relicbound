public class HighJumpRelic : Relic
{
    public HighJumpRelic()
        : base("relic.high_jump")
    {
    }

    public override void Apply(
        PlayerCapabilityController capabilityController
    )
    {
        capabilityController.AddCapability(
            new JumpHeightCapability(1.25f)
        );
    }
}