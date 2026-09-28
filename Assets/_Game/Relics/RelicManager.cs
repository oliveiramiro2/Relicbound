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

        foreach (Relic r in equipment.EquippedRelics)
        {
            Debug.Log(r);
        }

        StartCoroutine(RemoveRelic(highJumpRelic1));
    }

    private IEnumerator RemoveRelic(Relic highJumpRelic)
    {
        yield return new WaitForSeconds(5);
        equipment.Unequip(highJumpRelic);
        foreach (Relic r in equipment.EquippedRelics)
        {
            Debug.Log(r);
        }
    }

    public bool EquipRelic(Relic relic)
    {
        if (relic == null)
            return false;

        if (!inventory.Contains(relic.Id))
            return false;

        return equipment.Equip(relic);
    }
}