public class SlotExpansionRelic : Relic
{
    private readonly int slotAmount;

    public SlotExpansionRelic(int slotAmount)
        : base("relic.slot_expansion")
    {
        this.slotAmount = slotAmount;
    }

    public override void Apply(RelicContext context)
    {
        context.Equipment.AddSlots(slotAmount);
    }

    public override void Remove(RelicContext context)
    {
    }
}