
using UnityEngine;

/// <summary>
/// A data structure representing a "Class" (used in the RPG sense) that a soldier can be. IE:
/// Archer, wizard, fighter, etc. Editable by designers using the "ClassDatabase" singleton/prefab.
/// </summary>
[System.Serializable]
public class SoldierClassPrototype
{
    /// <summary>
    /// What to call this class collectively. IE: Archers.
    /// </summary>
    public string namePlural;

    /// <summary>
    /// What to call this class individually. IE: Archer.
    /// </summary>
    public string name;

    /// <summary>
    /// The image to portray this class.
    /// </summary>
    public Texture image;

    /// <summary>
    /// The image that is hue shifted and drawn on top of the image.
    /// </summary>
    public Texture swap;

    /// <summary>
    /// The base stats of this class, listed as HP, MP, ATK, DEF, MAG, RES, AGI, SPD.
    /// TODO: Can make this more intuitive with a custom inspector for this datastructure.
    /// </summary>
    public int[] stats = new int[8];
}