using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a wrapper around an array of SoldierClassPrototypes. This is so that a designer can, in the inspector, change the attributes
/// of all the classes without having to edit code. If it gets to unwieldy, I could make a custom inspector for it, but right now,
/// it seems to be easy enough to edit that it's not worth the time.
/// </summary>
public class ClassDatabase : MonoBehaviour
{
    private static ClassDatabase instance;

    /// <summary>
    /// The singleton instance of the class database.
    /// </summary>
    public static ClassDatabase Instance
    {
        get
        {

            if (!instance)
            {
                instance = GameObject.Instantiate<ClassDatabase>(Resources.Load<ClassDatabase>("Databases/ClassDatabase"));
            }
            return instance;
        }
    }

    /// <summary>
    /// An array containing the SoldierClassPrototypes for every soldier class in the game.
    /// </summary>
    public SoldierClassPrototype[] classes = new SoldierClassPrototype[0];
}
