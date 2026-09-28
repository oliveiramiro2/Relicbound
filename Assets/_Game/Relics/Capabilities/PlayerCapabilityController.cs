using UnityEngine;

public class PlayerCapabilityController : MonoBehaviour
{
    public PlayerCapabilities Capabilities { get; private set; }

    private void Awake()
    {
        Capabilities = new PlayerCapabilities();
    }

    public void AddCapability(PlayerCapability capability)
    {
        Capabilities.Add(capability);
    }

    public bool RemoveCapability(PlayerCapability capability)
    {
        return Capabilities.Remove(capability);
    }
}