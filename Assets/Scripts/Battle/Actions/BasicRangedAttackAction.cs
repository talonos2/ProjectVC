using System;
using UnityEngine;

/// <summary>
/// An action representing a bog-standard ranged attack. Not sure how often this will get used in practice, the VFX for ranged attacks
/// might necessitate different actions for different ranged attacks.
/// </summary>
internal class BasicRangedAttackAction : AIAction
{
    private Soldier target;
    private Soldier user;
    private int range;
    private float timeSinceStarted;

    private static bool init;

    //Math time.
    //Ranged attacks should deal less damage, both because of the damage density area/surface area problem, and for the alpha strike problem.
    //These numbers are based on the alpha strike problem. They essentially assume that the archer, having 7 greater range than a melee unit, get 3.5 "free" attacks.
    //In a normal combat situation (10 hits to kill) that means that you're dealing 1.35x as much damage as the enemy, so you should divide all attack powers by 1.35, then round.
    //This is all calculated in the "Stats" tab in the design spreadsheet.
    private static readonly float[][] basicAttackPowers = new float[17][]; 
    private static readonly float[] basicAttackCosts = new float[] { 6, 7, 9, 12, 15, 20, 26, 32, 42, 52, 68, 87, 112, 146, 179, 236, 297, 393, 493, 634, 856, 1092, 1338, 1768, 2220, 2692, 3686, 4737, 5837, 7567, 9999 };

    /// <summary>
    /// Creates a basic ranged attack action.
    /// </summary>
    /// <param name="target">The soldier getting shot</param>
    /// <param name="user">The soldier doing the shooting</param>
    /// <param name="range">The max range of this projectile.</param>
    public BasicRangedAttackAction(Soldier target, Soldier user, int range)
    {
        //TODO: google how to initialize spine-and-rib arrays in C#. Until then, initialize the hard-coded values in this array only once.
        if (!init)
        {
            init = true;
            basicAttackPowers[8] = new float[] { 20, 25, 30, 35, 40, 50, 60, 75, 90, 110, 135, 165, 205, 250, 300, 350, 450, 550, 650, 800, 1000, 1200, 1500, 1800, 2000, 2500, 3500, 4000, 5000, 6000, 7500 };
            basicAttackPowers[16] = new float[] { 14, 17, 21, 26, 32, 40, 50, 60, 70, 85, 105, 130, 155, 190, 250, 300, 350, 450, 500, 600, 800, 900, 1200, 1400, 1500, 2000, 2500, 3000, 4000, 4500, 5500 };
            basicAttackPowers[3] = new float[] { 25, 30, 35, 40, 50, 60, 75, 90, 110, 135, 170, 205, 250, 310, 350, 450, 550, 700, 850, 1000, 1200, 1500, 1800, 2200, 2500, 3500, 4000, 5000, 6000, 7500, 9000 };
        }
        this.target = target;
        this.user = user;
        this.range = range;
        PerformRangedAttack();
    }

    private void PerformRangedAttack()
    {
        //Calculate the damage this projectile will cause.
        float atkDefRatio = (float)user.stats.Att / (float)target.stats.Att;
        float power = basicAttackPowers[range][user.stats.BasicAttackLevel];
        float agiRatio = Mathf.Sqrt((float)user.stats.Agi / (float)target.stats.Agi);
        AttackResult hit = AttackResultUtils.TryToHit(agiRatio);
        float damage = atkDefRatio * hit.DamageMultiplier() * power;
        HitsplatInfo info = new HitsplatInfo
        {
            damage = (int)damage,
            type = DamageType.STAB,
            result = hit
        };

        //Send the projectile on its way. It handles its own animation, etc, after that.
        RangedProjectile projectile = GameObject.Instantiate<RangedProjectile>(user.basicRangedProjectile);
        projectile.Init(10, user.transform.position, target, info, (int)damage);

        user.currentMP -= (int)basicAttackCosts[user.stats.BasicAttackLevel];
    }

    internal override bool IsComplete()
    {
        if (timeSinceStarted > 1 / user.GetModifiedSpeed())
        {
            return true;
        }
        return false;
    }

    internal override void DoUpdate()
    {
        timeSinceStarted += Time.deltaTime;
    }
}