using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a barracks, a building that produces units (SoldierStats) with a number and quality based on its level.
/// </summary>
public class Barracks : Building
{
    /// <summary>
    /// What class (IE Archer, Wizard) this barracks produces.
    /// </summary>
    [SerializeField]
    public SoldierClassPrototype troopType;
    private int[] maxWorkersAtLevel = new int[]{1, 3, 7, 15, 30, 65, 130, 250, 510, 1025 }; //Often rounded because gamers like powers of two less than programmers.

    protected override void Start()
    {
        image.material = new Material(image.material);
        image.material.SetTexture("_face", troopType.image);
        image.material.SetTexture("_swap", troopType.swap);
        base.Start();
    }

    public override string GetClickText()
    {
        return "";
    }

    public override string GetProducesText()
    {
        return "";
    }

    public override string GetConsumesText()
    {
        return "";
    }

    public override int GetMaxInventoryCapacity()
    {
        return 0;
    }

    public override string GetDesc()
    {
        return "Trains and houses "+troopType.namePlural+"."+(Level >= 3?
            " The best eight recruits can be used in combat; the others stick around in hopes that a different "+troopType.name+" will retire and vacate a slot in the squad."
            :"");
    }

    protected override int GetMaxWorkers()
    {
        return maxWorkersAtLevel[Level];
    }

    public override string GetWorkersText()
    {
        return "Recruits:";
    }
}
