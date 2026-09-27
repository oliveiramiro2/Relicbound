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

        inventory.Add(doubleJumpRelic);

        bool equipped = equipment.Equip(doubleJumpRelic);

        Debug.Log(
            $"Double Jump equipado: {equipped}"
        );
    }

    public void Equip(Relic relic)
    {
        if (relic == null)
            return;

        equippedRelics.Add(relic);

        relic.Apply(capabilityController);
    }
}