using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCapabilityController))]
public class RelicManager : MonoBehaviour
{
    private PlayerCapabilityController capabilityController;
    private RelicInventory inventory;
    private RelicEquipment equipment;

    private readonly List<Relic> equippedRelics = new();

    private void Awake()
    {
        capabilityController =
            GetComponent<PlayerCapabilityController>();

        inventory = new RelicInventory();
        equipment = new RelicEquipment(
                        2,
                        capabilityController
                    );
    }

    private void Start()
    {
        Relic doubleJumpRelic = new DoubleJumpRelic();
        Relic highJumpRelic = new HighJumpRelic();

        inventory.Add(doubleJumpRelic);
        inventory.Add(highJumpRelic);

        equipment.Equip(doubleJumpRelic);
        equipment.Equip(highJumpRelic);
    }

    public void Equip(Relic relic)
    {
        if (relic == null)
            return;

        equippedRelics.Add(relic);

        relic.Apply(capabilityController);
    }
}