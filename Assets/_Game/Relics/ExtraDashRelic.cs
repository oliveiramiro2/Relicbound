public class ExtraDashRelic : Relic
{
  private ExtraDashCapability capability;

  public ExtraDashRelic()
      : base("relic.extra_dash")
  {
  }

  public override void Apply(RelicContext context)
  {
    capability = new ExtraDashCapability();

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