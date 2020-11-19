using System;
using UnityEngine;

/// <summary>
/// Contains all the persistent data regarding a given soldier.
/// </summary>
public class SoldierStats
{
    private int hp = 10;
    private int mp = 10;
    private int att = 10;
    private int def = 10;
    private int mag = 10;
    private int res = 10;
    private int spd = 10;
    private int agi = 10;
    private int basicAttackLevel = 0;

    private SoldierClassPrototype soldierClass;
    private float talent;
    private int level = 1;

    //Self-explanitory read-only values.
    public int Hp { get => hp; }
    public int Mp { get => mp; }
    public int Att { get => att; }
    public int Def { get => def; }
    public int Mag { get => mag; }
    public int Res { get => res; }
    public int Spd { get => spd; }
    public int Agi { get => agi; }
    public int BasicAttackLevel { get => basicAttackLevel; }
    public int Level { get => level; }

    /// <summary>
    /// The name of this soldier.
    /// </summary>
    public string name;

    //Fields having to do with metagame progression. My eyes are bigger than my stomach. 
    //TODO all of this, lol.

    //private int evs;
    //private Armor armor;
    //private Weapon weapon;
    //private int imbument;
    //private float morale;
    //private SoldierStats inRelationshipWith;
    //private int relationshipLevel;
    //private PlayerData owningPlayer (Contains chapter crystals)
    //private float screenTime
    //private float crosstraining
    //private float spiritPower
    //private float easterEggBuffs

    /// <summary>
    /// Recalculate all cached stats. Should be called every time the persistent data of this soldier becomes dirty.
    /// </summary>
    public void RecalculateStats()
    {
        hp = (int)(soldierClass.stats[0]/10.0f * (Level + 9) * talent*10);
        mp = (int)(Mathf.Pow(soldierClass.stats[1] * (Level + 9) * talent,1.2500125f));
        att = (int)(soldierClass.stats[2] / 10.0f * (Level + 9) * talent);
        def = (int)(soldierClass.stats[3] / 10.0f * (Level + 9) * talent);
        mag = (int)(soldierClass.stats[4] / 10.0f * (Level + 9) * talent);
        res = (int)(soldierClass.stats[5] / 10.0f * (Level + 9) * talent);
        agi = (int)(soldierClass.stats[6] / 10.0f * (Level + 9) * talent);
        spd = (int)(soldierClass.stats[7] / 10.0f * (Level + 9) * talent);
    }

    /// <summary>
    /// Creates a new SoldierStats. TODO: This eventually should only be called by the metagame progression.
    /// </summary>
    /// <param name="prototype">The class (job) prototype of this soldier.</param>
    /// <param name="talent">The base talent of this soldier, given by the Barracks this soldier came from.</param>
    /// <param name="name">The name of this soldier.</param>
    public SoldierStats(SoldierClassPrototype prototype, float talent, String name)
    {
        this.soldierClass = prototype;
        this.talent = talent;
        this.name = name;
        RecalculateStats();
    }

    //Sets the level of this soldier.
    public void SetLevel(int level)
    {
        this.level = level;
        RecalculateStats();
    }

    float PD = 21.54219552f; // Divide each of a perfect unit's stats by this, and you get 9999 trillion. (Stands for Power Divisor)

    /// <summary>
    /// Gets the power of the soldier. Power is weird: It's not actually a stat. Rather, it represents rougly how good a soldier is
    /// in total. The min is around 30, and the max is 9999 trillion. This is because power is basically all stats multiplied together.
    /// Why? Well, if all of Soldier A's stats are 2x as good as soldier B's stats, then A will deal twice the damage, have 2x as much HP
    /// and take 1/2 damage from all attacks, act 2x as often, and dodge √2 as often and crit √2 as often and have the MP to use skills that
    /// deal 2x as much damage. Thus, a unit's power rises with the sixth power of its stats, which is why power can get so insanely high if 
    /// you cap all stats (9999 of each.) This is why a 5% boost to all stats is actually a MAJOR benefit.
    /// 
    /// A troop with 2x the power of another troop should end a battle with roughly half its health remaining.
    /// </summary>
    /// <returns>The power of this troop.</returns>
    internal float GetTotalPower()
    {
        float power = (hp/(PD*10)) * (mp/PD) * (Math.Max(att, def)/PD) * ((def + mag / 2)/PD) * (spd/PD) * (agi/PD);
        power = Mathf.RoundToInt(power);
        return power;
    }

    /// <summary>
    /// Gets the name of this Soldier's class.
    /// </summary>
    /// <returns></returns>
    internal string GetClassName()
    {
        return soldierClass.name;
    }

    //Gets the class prototype for this soldier. TODO: Make this immutable somehow.
    internal SoldierClassPrototype GetPrototype()
    {
        return soldierClass;
    }
}