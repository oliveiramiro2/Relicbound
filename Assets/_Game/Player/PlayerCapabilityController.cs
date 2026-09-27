using UnityEngine;

public class PlayerCapabilityController : MonoBehaviour
{
    public PlayerCapabilities Capabilities { get; private set; }

    private void Awake()
    {
        Capabilities = new PlayerCapabilities();
    }
}