using System;
using System.Collections.Generic;

[Serializable]
public class RelicSaveData
{
    public List<string> inventoryRelicIds = new();
    public List<string> equippedRelicIds = new();

    public int slotCount;
}