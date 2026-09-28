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
        Relic highJumpRelic1 = new HighJumpRelic();
        Relic highJumpRelic2 = new HighJumpRelic();

        inventory.Add(doubleJumpRelic);
        inventory.Add(highJumpRelic1);
        inventory.Add(highJumpRelic2);

        bool first = equipment.Equip(highJumpRelic1);
        bool second = equipment.Equip(highJumpRelic1);
        bool third = equipment.Equip(highJumpRelic2);

        Debug.Log($"first: {first} - second: {second} - third: {third}");
    }

    public void Equip(Relic relic)
    {
        if (relic == null)
            return;

        equippedRelics.Add(relic);

        relic.Apply(capabilityController);
    }
}