using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows cardinal unit vectors to be represented as an Enum.
/// </summary>
public enum Direction
{
    NORTH, SOUTH, EAST, WEST, INVALID
}

/// <summary>
/// A class with static methods pertaining to Direction enums.
/// </summary>
public class DirectionUtils
{
    /// <summary>
    /// Gets a random cardinal direction
    /// </summary>
    /// <returns>One of {NORTH, SOUTH, EAST, WEST}</returns>
    public static Direction Get()
    {
        int random = UnityEngine.Random.Range(0, 4);
        return (Direction)random;
    }

    /// <summary>
    /// Converts a vector into a unit direction.
    /// </summary>
    /// <param name="dir">The vector to change into a direction</param>
    /// <returns>The direction represented by a unit vector, or INVALID if it is not a cardinal unit vector.</returns>
    public static Direction VectorToDirection(Vector2Int dir)
    {
        if (dir == Vector2Int.up) { return Direction.NORTH; }
        if (dir == Vector2Int.down) { return Direction.SOUTH; }
        if (dir == Vector2Int.left) { return Direction.WEST; }
        if (dir == Vector2Int.right) { return Direction.EAST; }
        Debug.LogError("VectorToDirection failed with an input of " + dir);
        return Direction.INVALID;
    }
}

/// <summary>
/// A class with extensions to the Direction enums, allowing us to treat them more like flyweights than enums.
/// </summary>
static class DirectionExtensions
{
    /// <summary>
    /// Converts a direction into a unit vector.
    /// </summary>
    /// <param name="dir">The direction to change into a vector</param>
    /// <returns>The corresponding Unit Vector</returns>
    public static Vector2Int Vector(this Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return Vector2Int.up;
            case Direction.SOUTH:
                return Vector2Int.down;
            case Direction.EAST:
                return Vector2Int.right;
            case Direction.WEST:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }



    /// <summary>
    /// Gets the opposite of a Direction.
    /// </summary>
    /// <param name="dir">The Direction to get the opposite of.</param>
    /// <returns>The opposite of that Direction.</returns>
    public static Direction Opposite(this Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
            default:
                return Direction.INVALID;
        }
    }
}

/// <summary>
/// Because of the way the accuracy system works, we need many "levels" of critical hits. These are grouped into types: Criticals,
/// Vital strikes, and deathblows of three different types, all of which can be mixed and matched.
/// 
/// In practice, you probably won't see anything higher than a deathblow, but in theory, if you have a unit with 9999 agility attack
/// a unit with 10 agility, you can end up seeing the higher-end ones. Even then, without abilities that modify crit chance directly
/// or provide agility buffs/debuffs, you probably won't see anything higher than a critical mega deathblow.
/// </summary>
public enum AttackResult
{
    MISS, NICK, WEAK_HIT, HIT, STRONG_HIT, CRIT,VITAL_STRIKE, DEATHBLOW,
    CRITICAL_VITAL_STRIKE, MEGA_DEATHBLOW, CRITICAL_DEATHBLOW, ULTIMATE_DEATHBLOW, VITAL_STRIKE_DEATHBLOW,  CRITICAL_MEGA_DEATHBLOW,
    VITAL_STRIKE_MEGA_DEATHBLOW, CRITICAL_ULTIMATE_DEATHBLOW, CRITICAL_VITAL_STRIKE_DEATHBLOW, VITAL_STRIKE_ULTIMATE_DEATHBLOW,
    CRITICAL_VITAL_STRIKE_MEGA_DEATHBLOW, CRITICAL_VITAL_STRIKE_ULTIMATE_DEATHBLOW, INVALID
}

/// <summary>
/// A class with extensions to the Attack Result enum, allowing us to treat it more like a flyweight than an enum.
/// </summary>
static class AttackResultExtensions
{
    /// <summary>
    /// Gets the damage multiplier for an attack result.
    /// </summary>
    /// <param name="dir">The AttackResult to get the damage from.</param>
    /// <returns>The damage multiplier on the Attack Result.</returns>
    public static float DamageMultiplier(this AttackResult dir)
    {
        switch (dir)
        {
            case AttackResult.MISS:
                return 0;
            case AttackResult.NICK:
                return .25f;
            case AttackResult.WEAK_HIT:
                return .75f;
            case AttackResult.HIT:
                return 1f;
            case AttackResult.STRONG_HIT:
                return 1.25f;
            case AttackResult.CRIT:
                return 2.5f;
            case AttackResult.VITAL_STRIKE:
                return 4f;
            case AttackResult.DEATHBLOW:
                return 8f;
            case AttackResult.CRITICAL_VITAL_STRIKE:
                return 10f;
            case AttackResult.MEGA_DEATHBLOW:
                return 15f;
            case AttackResult.CRITICAL_DEATHBLOW:
                return 20f;
            case AttackResult.ULTIMATE_DEATHBLOW:
                return 25f;
            case AttackResult.VITAL_STRIKE_DEATHBLOW:
                return 32f;
            case AttackResult.CRITICAL_MEGA_DEATHBLOW:
                return 37.5f;
            case AttackResult.VITAL_STRIKE_MEGA_DEATHBLOW:
                return 60f;
            case AttackResult.CRITICAL_ULTIMATE_DEATHBLOW:
                return 67.5f;
            case AttackResult.CRITICAL_VITAL_STRIKE_DEATHBLOW:
                return 80f;
            case AttackResult.VITAL_STRIKE_ULTIMATE_DEATHBLOW:
                return 100f;
            case AttackResult.CRITICAL_VITAL_STRIKE_MEGA_DEATHBLOW:
                return 150f;
            case AttackResult.CRITICAL_VITAL_STRIKE_ULTIMATE_DEATHBLOW:
                return 250f;
            default:
                return -1;
        }
    }

    /// <summary>
    /// Checks whether or not an AttackResult has the "Critical" component in it.
    /// </summary>
    /// <param name="result">The AttackResult to check.</param>
    /// <returns>Whether or not this AttackResult is a crit.</returns>
    public static bool IsCrit(this AttackResult result)
    {
        return result==AttackResult.CRIT||
            result == AttackResult.CRITICAL_DEATHBLOW||
            result == AttackResult.CRITICAL_MEGA_DEATHBLOW||
            result == AttackResult.CRITICAL_ULTIMATE_DEATHBLOW ||
            result == AttackResult.CRITICAL_VITAL_STRIKE ||
            result == AttackResult.CRITICAL_VITAL_STRIKE_DEATHBLOW ||
            result == AttackResult.CRITICAL_VITAL_STRIKE_MEGA_DEATHBLOW ||
            result == AttackResult.CRITICAL_VITAL_STRIKE_ULTIMATE_DEATHBLOW;
    }
}

/// <summary>
/// A class containing helper methods that pertain to AttackResults.
/// </summary>
public class AttackResultUtils
{
    /// <summary>
    /// These numbers are derived in the "Accuracy Calcs" tab of the design spreadsheet.
    /// </summary>
    private static readonly float[] hitBrackets = new float[] { 0, 0.0125f, 0.0875f, 0.325f, 0.6875f, 1.0375f,
        1.5625f, 2.75f, 4.6625f, 7.675f, 10.8f, 15.15f, 20.1f, 25.525f, 32.5f, 41.775f, 56.6f, 68.625f, 84f,
        111.875f, 161.5f, 222.5f, 245, 250, 100000000 };

    /// <summary>
    /// Creates a random hit type, dependant on tht ratio between the attacker and defender's agility.
    /// 
    /// Basically, it finds what type of hit the attacker should get "on average", then adjusts the hit
    /// type up for down by up to two categories to allow for random variation.
    /// 
    /// It figures out what the base hit type is based on the average damage for each hit type. For instance,
    /// for the base AttackResult HIT, there's a 50% chance of a HIT, a 20% chance of a WEAK_HIT, a 5% chance
    /// of a NICK, a 20% chance of a STRONG_HIT, and a 5% chance of a CRIT, equalling an average damage multiplier
    /// of 1.0375. This, if agiRatio = 1.0375, then we pick a base AttackResult if "HIT".
    /// 
    /// If the AGI ratio falls between two hit types, then it randomly picks either the higher or lower one, weighted
    /// according to how close the agiRatio is towards one or the other.
    /// </summary>
    /// <param name="agiRatio"></param>
    /// <returns></returns>
    public static AttackResult TryToHit(float agiRatio)
    {
        int x = 0;
        while (agiRatio > hitBrackets[x])
        {
            x++;
        }
        int result = x - 3; //The lowest three numbers corespond to results such as MISS, MISS, MISS, 
                            //MISS, NICK; basically a *lower* category than a normal miss.
        float dieRoll = UnityEngine.Random.value;
        if (dieRoll < .05)
        {
            result -= 2;
        }
        else if (dieRoll < .25)
        {
            result -= 1;
        }
        else if (dieRoll < .75)
        {
            //NOOP
        }
        else if (dieRoll < .95)
        {
            result += 1;
        }
        else
        {
            result += 2;
        }
        float widthOfRange = hitBrackets[x] - hitBrackets[x - 1];
        float distanceToNext = agiRatio - hitBrackets[x - 1];
        float chanceOfUpgrade = distanceToNext / widthOfRange;
        if (UnityEngine.Random.value > chanceOfUpgrade)
        {
            result += 1;
        }
        result = Mathf.Clamp(result, 0, 20);
        return (AttackResult)result;
    }
}

/// <summary>
/// What damage type a source of damage is inflicting.
/// </summary>
public enum DamageType
{
    SLASH, STAB, CRUSH, INVALID
}

/// <summary>
/// Is this owned by the player or Enemy?
/// 
/// If we ever do battles with more than two sides, we'll need to add more entries here.
/// </summary>
public enum Owner
{
    PLAYER,
    ENEMY
}
