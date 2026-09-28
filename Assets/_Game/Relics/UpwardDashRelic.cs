public class UpwardDashRelic : Relic
{
  private DirectionalDashCapability capability;

  public UpwardDashRelic()
      : base("relic.upward_dash")
  {
  }

  public override void Apply(RelicContext context)
  {
    capability = new DirectionalDashCapability();

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