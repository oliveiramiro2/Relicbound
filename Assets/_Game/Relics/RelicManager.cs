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
        equipment.AddSlots(2);

        TestRelicSystem();
    }

    private void TestRelicSystem()
    {
        Relic doubleJumpRelic = new DoubleJumpRelic();
        Relic highJumpRelic1 = new HighJumpRelic();
        Relic highJumpRelic2 = new HighJumpRelic();
        Relic speedRelic = new SpeedRelic();
        Relic longDashRelic1 = new LongDashRelic();
        Relic longDashRelic2 = new LongDashRelic();
        Relic upwardDashRelic = new UpwardDashRelic();

        AddRelic(doubleJumpRelic);
        AddRelic(highJumpRelic1);
        AddRelic(highJumpRelic2);
        AddRelic(speedRelic);
        AddRelic(longDashRelic1);
        AddRelic(longDashRelic2);
        AddRelic(upwardDashRelic);

        EquipRelic(speedRelic);
        EquipRelic(longDashRelic2);
        EquipRelic(longDashRelic1);
        EquipRelic(upwardDashRelic);

        foreach (Relic relic in equipment.EquippedRelics)
        {
            Debug.Log($"Equipped: {relic.Id}");
        }

        //StartCoroutine(RemoveRelic(speedRelic));
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