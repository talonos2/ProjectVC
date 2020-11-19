using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a farm building. Produces food.
/// </summary>
public class CropFields : Building
{
    [SerializeField]
    private int[] maxWorkersAtLevel = new int[]{24, 40, 75, 120, 225, 375, 705, 1200, 2250, 3870 };

    protected override void Start()
    {
        activeWorkers = 10;
        base.Start();
        inventory.Add(InventoryItem.GRAIN, 30);
        inventory.Add(InventoryItem.FLAX, 25);
        inventory.Add(InventoryItem.HOPS, 20);
    }
    public override string GetClickText()
    {
        return "";
    }

    public override string GetProducesText()
    {
        return "Produces: " + (activeWorkers * GetGrainProduction()) + " "+GetShortProduced()+" per day.";
    }

    public override string GetConsumesText()
    {
        return "";
    }

    public override int GetMaxInventoryCapacity()
    {
        return maxWorkersAtLevel[Level] * GetGrainProduction();
    }

    public override string GetDesc()
    {
        return "Generates grain. Each farmer will generate "+GetGrainProduction()+" "+GetGrainsProduced()+" per day.";
    }

    private int GetGrainProduction()
    {
        return (int)(5.5f + (float)Level/2.0f);
    }

    protected override int GetMaxWorkers()
    {
        return maxWorkersAtLevel[Level];
    }

    private string GetGrainsProduced()
    {
        if (Level < 1)
        {
            return InventoryItem.GRAIN.GetTextCode();
        }
        if (Level < 2)
        {
            return InventoryItem.GRAIN.GetTextCode()+" or "+InventoryItem.FLAX.GetTextCode();
        }
        return InventoryItem.GRAIN.GetTextCode()+", "+ InventoryItem.FLAX.GetTextCode()+", or "+ InventoryItem.HOPS.GetTextCode();
    }

    private string GetShortProduced()
    {
        if (Level < 1)
        {
            return InventoryItem.GRAIN.GetTextCode();
        }
        if (Level < 2)
        {
            return InventoryItem.GRAIN.GetTextCode() + "/" + InventoryItem.FLAX.GetTextCode();
        }
        return InventoryItem.GRAIN.GetTextCode() + "/" + InventoryItem.FLAX.GetTextCode() + "/" + InventoryItem.HOPS.GetTextCode();
    }

    public override string GetWorkersText()
    {
        return "Farmers:";
    }
}
