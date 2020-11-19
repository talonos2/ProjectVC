using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for a building.
/// </summary>
public class Building : MonoBehaviour
{
    /// <summary>
    /// How large the building is.
    /// </summary>
    public Vector2Int size = new Vector2Int(1,1);

    /// <summary>
    /// Where the building is
    /// </summary>
    public Vector2Int position = new Vector2Int(1, 1);

    /// <summary>
    /// The GO containing the building's graphic.
    /// </summary>
    public Transform quad;

    /// <summary>
    /// The renderer that displays the image associated with this building.
    /// </summary>
    public MeshRenderer image;

    /// <summary>
    /// How many workers are cirrently assigned to this building.
    /// </summary>
    public int activeWorkers;

    protected Dictionary<InventoryItem, int> inventory = new Dictionary<InventoryItem, int>();
    private TownMapController controller;
    private int level = 0;

    /// <summary>
    /// What level this building is
    /// </summary>
    public int Level { get => level; }

    /// <summary>
    /// How many workers this building can support.
    /// </summary>
    public int MaxWorkers { get => GetMaxWorkers();}

    protected virtual int GetMaxWorkers()
    {
        return 1;
    }

    /// <summary>
    /// Returns how many items, total, there are in this building's inventory.
    /// </summary>
    /// <returns></returns>
    public int GetAmountInInventory()
    {
        int amount = 0;
        foreach (InventoryItem i in inventory.Keys)
        {
            amount += inventory[i];
        }
        return amount;
    }

    /// <summary>
    /// Returns a KeyCollection of the types of items contained in this building's inventory.
    /// </summary>
    /// <returns></returns>
    public Dictionary<InventoryItem,int>.KeyCollection GetTypesContainedInInventory()
    {
        return inventory.Keys;
    }

    /// <summary>
    /// Returns how many items of a given type there are in the building's inventory.
    /// </summary>
    /// <param name="i">The type of item to check for.</param>
    /// <returns></returns>
    public int GetAmountInInventory(InventoryItem i)
    {
        return inventory[i];
    }

    /// <summary>
    /// Returns how many items this building can hold, maximum.
    /// </summary>
    /// <returns></returns>
    public virtual int GetMaxInventoryCapacity()
    {
        throw new NotImplementedException();
    }

    protected virtual void Start()
    {
        this.transform.localScale = new Vector3(size.x, 1, size.y);
    }

    public void Update()
    {
        
    }

    /// <summary>
    /// Initializes a building to a given location.
    /// </summary>
    /// <param name="townMapController">A pointer to the controller that oversees this building.</param>
    /// <param name="x">The x location of this building.</param>
    /// <param name="y">The y location of this building. TODO: Why isn't this a Vector2Int?</param>
    public void Init(TownMapController townMapController, int x, int y)
    {
        this.controller = townMapController;
        this.position = new Vector2Int(x, y);
        this.transform.localPosition = new Vector3(x, .001f, y);
    }

    /// <summary>
    /// What the bottom-most text box in the building's tooltip says. Usually "Click for more info!"
    /// </summary>
    /// <returns></returns>
    public virtual string GetClickText()
    {
        return "Click for more info!";
    }

    /// <summary>
    /// What the "Produces" section of the tooltip says.
    /// </summary>
    /// <returns></returns>
    public virtual string GetProducesText()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// What the "Consumes" section of the tooltip says.
    /// </summary>
    /// <returns></returns>
    public virtual string GetConsumesText()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// What the descriptive text of this building says.
    /// </summary>
    /// <returns></returns>
    public virtual string GetDesc()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// What the workers text of this building says. Changes depending on the building. IE: "Butchers", "Bakers", "Candlestick Makers"
    /// </summary>
    /// <returns></returns>
    public virtual string GetWorkersText()
    {
        return "Workers";
    }

    /// <summary>
    /// What the "Storage" section of the tooltip says. Usually doesn't need to change.
    /// </summary>
    /// <returns></returns>
    public virtual string GetStorageText()
    {
        return "Storage";
    }

    /// <summary>
    /// Gets the string representing how full the storage of this building is. Usually doesn't need to change.
    /// </summary>
    /// <returns></returns>
    public string GetStorageAmountText()
    {
        return GetAmountInInventory()+"/"+GetMaxInventoryCapacity();
    }
}
