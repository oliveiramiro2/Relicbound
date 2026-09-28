public class RelicContext
{
    public PlayerCapabilityController CapabilityController { get; }
    public RelicEquipment Equipment { get; }

    public RelicContext(
        PlayerCapabilityController capabilityController,
        RelicEquipment equipment
    )
    {
        CapabilityController = capabilityController;
        Equipment = equipment;
    }
}