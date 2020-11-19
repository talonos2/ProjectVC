using UnityEngine;

/// <summary>
/// An Enum containing all possible items in the game that a building can hold.
/// Note that armor, weapons, etc are not considered "Items", but "Equipment".
/// TODO: Maybe we could put this in a database, like the ClassDatabase, to make it more easily
/// edited by designers?
/// </summary>
public enum InventoryItem
{
    GOLD,
    FOOD,
    MANA,
    SPIRIT_ESSENCE,
    LUMBER,
    LIGHT_LUMBER,
    HEAVY_LUMBER,
    PRISTINE_LUMBER,
    WOOD,
    FLEXWOOD,
    IRONWOOD,
    GODWOOD,
    SANDSTONE,
    GRANITE,
    QUARTZITE,
    OBSIDIAN,
    SANDSTONE_BRICKS,
    GRANITE_BRICKS,
    QUARTZITE_BRICKS,
    OBSIDIAN_BRICKS,
    IRON_ORE,
    MYTHRIL_ORE,
    ADAMANTINE_ORE,
    ORICHALCUM_ORE,
    STEEL,
    MYTHRIL,
    ADAMANTINE,
    ORICHALCUM,
    HIDE,
    MONSTERHIDE,
    DRAGONHIDE,
    MYTHIDE,
    LEATHER,
    MONSTER_LEATHER,
    DRACONIC_LEATHER,
    MYTHICAL_LEATHER,
    FLAX,
    SILK_THREADS,
    LINEN,
    SILK_CLOTH,
    MYTHRILWEAVE,
    GODWEAVE,
    BONE,
    HOLLOWBONE,
    IRONBONE,
    DEMONBONE,
    REPTILE_SCALES,
    HEAVY_SCALES,
    MONSTER_SCALES,
    DRAGON_SCALES,
    GRAIN,
    MEAT,
    HOPS,
    FEATHERS,
    EAGLE_FEATHERS,
    MONSTER_FEATHERS,
    ANGELFEATHERS,
    RAW_QUARTZ,
    RAW_JET,
    RAW_TOPAZ,
    RAW_AMETHYST,
    RAW_SAPPHIRE,
    RAW_RUBY,
    RAW_EMERALD,
    RAW_DIAMOND,
    QUARTZ,
    JET,
    TOPAZ,
    AMETHYST,
    SAPPHIRE,
    RUBY,
    EMERALD,
    DIAMOND,
    BOOZE,
    HERBS,
    MEDICINE,
    SALVE,
    LESSER_HEALING,
    HEALING,
    MAJOR_HEALING,
    SUPREME_HEALING,
    ELIXIR,
    REVITALIZATION,
    REVIVIFICATION,
    DIVINE_CURE
}

/// <summary>
/// A class containing extension methods for inventory items so we can treat them more like Flyweights.
/// </summary>
public static class InventoryItemExtensions
{
    /// <summary>
    /// A method to get a text code for the item's icon, usable in TextMeshPro.
    /// </summary>
    /// <param name="item">The item whose text code we want to retrieve.</param>
    /// <returns></returns>
    public static string GetTextCode(this InventoryItem item)
    {
        int index = 0;
        switch (item)
        {
            case InventoryItem.GRAIN:
            case InventoryItem.HOPS:
            case InventoryItem.FLAX:
                index = 1; break;
            case InventoryItem.FOOD:
                index = 2; break;
            case InventoryItem.GOLD:
                index = 3; break;
        };
        return "<sprite index="+index+" color=#" + ColorUtility.ToHtmlStringRGB(item.GetColor()) + ">";
    }

    /// <summary>
    /// The color associated with the item. Used int the icon color, the bar color, etc.
    /// </summary>
    /// <param name="item">The item whose color we want to retrieve.</param>
    /// <returns></returns>
    public static Color GetColor(this InventoryItem item)
    {
        switch (item)
        {
            case InventoryItem.GRAIN:
            case InventoryItem.FOOD:
                return new Color(0.949f, 0.878f, 0.322f);
            case InventoryItem.GOLD:
                return new Color(1, .886f, 0);
            case InventoryItem.HOPS:
            case InventoryItem.BOOZE:
                return new Color(0.716f, 0.530f, 0);
            case InventoryItem.FLAX:
            case InventoryItem.LINEN:
                return new Color(1, .975f, .646f);
            default:
                return Color.white;
        }
    }
}