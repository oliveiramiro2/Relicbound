using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCapabilityController))]
public class RelicManager : MonoBehaviour
{
    private PlayerCapabilityController capabilityController;
    private RelicInventory inventory;
    private RelicEquipment equipment;

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
        equipment.AddSlots(1);

        Relic doubleJumpRelic = new DoubleJumpRelic();
        Relic highJumpRelic1 = new HighJumpRelic();
        Relic highJumpRelic2 = new HighJumpRelic();
        Relic speedRelic = new SpeedRelic();

        inventory.Add(doubleJumpRelic);
        inventory.Add(highJumpRelic1);
        inventory.Add(speedRelic);

        EquipRelic(speedRelic);
        EquipRelic(highJumpRelic1);
        EquipRelic(highJumpRelic2);

        inventory.Add(highJumpRelic2);

        foreach (Relic r in equipment.EquippedRelics)
        {
            Debug.Log(r);
        }

        StartCoroutine(RemoveRelic(highJumpRelic1, highJumpRelic2));
    }

    private IEnumerator RemoveRelic(Relic relic, Relic relic2)
    {
        yield return new WaitForSeconds(5);

        bool removed = UnequipRelic(relic);

        Debug.Log($"Unequip result: {removed}");

        EquipRelic(relic2);

        foreach (Relic equippedRelic in equipment.EquippedRelics)
        {
            Debug.Log(equippedRelic.Id);
        }
    }

    public bool EquipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (!inventory.Contains(relic))
            return false;

        return equipment.Equip(relic);
    }

    public bool UnequipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        return equipment.Unequip(relic);
    }
}