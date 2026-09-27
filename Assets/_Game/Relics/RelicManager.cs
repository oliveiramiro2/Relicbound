using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCapabilityController))]
public class RelicManager : MonoBehaviour
{
    private PlayerCapabilityController capabilityController;

    private readonly List<Relic> equippedRelics = new();

    private void Awake()
    {
        capabilityController =
            GetComponent<PlayerCapabilityController>();
    }

    void Start()
    {
        Equip(new DoubleJumpRelic());
    }

    public void Equip(Relic relic)
    {
        if (relic == null)
            return;

        equippedRelics.Add(relic);

        relic.Apply(capabilityController);
    }
}