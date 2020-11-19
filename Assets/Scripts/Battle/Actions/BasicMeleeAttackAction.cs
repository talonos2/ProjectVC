using System;
using UnityEngine;

/// <summary>
/// An action representing a melee attack.
/// </summary>
internal class BasicMeleeAttackAction:AIAction
{
    float timeSinceStarted;
    private Soldier target;
    private Soldier user;

    // These are all derived on the "Stats" page of the design spreadsheet. TODO: BasicAttackPowers is wrong. Renormalize it to 9999 power instead of 99999
    private static readonly float[] basicAttackPowers = new float[] {25,35,45,60,75,100,130,175,230,300,400,525,700,900,1200,1600,2100,2750,3600,4800,6300,8300,11000,14500,19000,25000,33000,44000,58000,76000,99999 };
    private static readonly float[] basicAttackCosts = new float[] { 6, 7, 9, 12, 15, 20, 26, 32, 42, 52, 68, 87, 112, 146, 179, 236, 297, 393, 493, 634, 856, 1092, 1338, 1768, 2220, 2692, 3686, 4737, 5837, 7567, 9999 };

    /// <summary>
    /// Creates a new melee attack action.
    /// </summary>
    /// <param name="target">The hit Soldier</param>
    /// <param name="soldier">The soldier doing the hitting.</param>
    public BasicMeleeAttackAction(Soldier target, Soldier soldier)
    {
        this.target = target;
        this.user = soldier;
        PerformMeleeAttack();
    }

    private void PerformMeleeAttack()
    {
        float atkDefRatio = (float)user.stats.Att/ (float)target.stats.Att;
        float power = basicAttackPowers[user.stats.BasicAttackLevel];
        float agiRatio = Mathf.Sqrt((float)user.stats.Agi / (float)target.stats.Agi);
        AttackResult hit = AttackResultUtils.TryToHit(agiRatio);
        float damage = atkDefRatio*hit.DamageMultiplier()*power;
        target.currentHP -= (int)damage;
        user.currentMP -= (int) basicAttackCosts[user.stats.BasicAttackLevel];
        HitsplatInfo info = new HitsplatInfo
        {
            damage = (int)damage,
            type = DamageType.SLASH,
            result = hit
        };
        target.CreateHitsplat(info);
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