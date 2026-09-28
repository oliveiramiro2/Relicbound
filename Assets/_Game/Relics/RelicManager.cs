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

        TestRelicSystem();
    }

    private void TestRelicSystem()
    {
        Relic doubleJumpRelic = new DoubleJumpRelic();
        Relic highJumpRelic1 = new HighJumpRelic();
        Relic highJumpRelic2 = new HighJumpRelic();
        Relic speedRelic = new SpeedRelic();
        Relic longDashRelic = new LongDashRelic();

        AddRelic(doubleJumpRelic);
        AddRelic(highJumpRelic1);
        AddRelic(highJumpRelic2);
        AddRelic(speedRelic);
        AddRelic(longDashRelic);

        bool speedEquipped = EquipRelic(speedRelic);
        bool highJump1Equipped = EquipRelic(highJumpRelic1);
        bool highJump2Equipped = EquipRelic(longDashRelic);

        Debug.Log($"Speed equipped: {speedEquipped}");
        Debug.Log($"HighJump 1 equipped: {highJump1Equipped}");
        Debug.Log($"HighJump 2 equipped: {highJump2Equipped}");

        foreach (Relic relic in equipment.EquippedRelics)
        {
            Debug.Log($"Equipped: {relic.Id}");
        }

        StartCoroutine(RemoveRelic(longDashRelic));
    }

    private IEnumerator RemoveRelic(Relic relic)
    {
        yield return new WaitForSeconds(15);

        bool removed = UnequipRelic(relic);

        Debug.Log($"Unequip result: {removed}");

        foreach (Relic equippedRelic in equipment.EquippedRelics)
        {
            Debug.Log(equippedRelic.Id);
        }
    }

    public bool AddRelic(Relic relic)
    {
        if (relic == null)
            return false;

        inventory.Add(relic);
        return true;
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