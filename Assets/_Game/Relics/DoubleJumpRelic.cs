public class DoubleJumpRelic : Relic
{
    public DoubleJumpRelic()
        : base("relic.double_jump")
    {
    }

    public override void Apply(
        PlayerCapabilityController capabilityController
    )
    {
        capabilityController.AddCapability(
            new DoubleJumpCapability()
        );
    }
}