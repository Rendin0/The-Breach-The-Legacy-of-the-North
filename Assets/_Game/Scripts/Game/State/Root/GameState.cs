using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public int GlobalEntityId;
    public List<CreatureEntity> Creatures;
    public List<InventoryGridData> Inventories;

    public int GetEntityId()
    {
        return GlobalEntityId++;
    }

}