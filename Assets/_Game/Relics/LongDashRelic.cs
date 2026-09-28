public class LongDashRelic : Relic
{
  private DashImpulseCapability capability;

  public LongDashRelic()
      : base("relic.long_dash")
  {
  }

  public override void Apply(RelicContext context)
  {
    capability = new DashImpulseCapability(1.5f);

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