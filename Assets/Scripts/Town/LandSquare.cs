using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a plot of land upon which you can wage battles or build buildings.
/// </summary>
public class LandSquare : MonoBehaviour
{
    private bool owned;

    /// <summary>
    /// A pointer to the TownMapController that controls this land.
    /// </summary>
    public TownMapController controller;

    /// <summary>
    /// Is this land visible, or undiscovered?
    /// </summary>
    public bool visible;

    /// <summary>
    /// Where this land is.
    /// </summary>
    public Vector2Int position;

    /// <summary>
    /// How quickly this plot of land raises when discovered.
    /// </summary>
    public float raiseSpeed = .01f;

    /// <summary>
    /// What material is rendered on this plot of land when it is owned by a player.
    /// </summary>
    public Material ownedMaterial;

    /// <summary>
    /// What material is rendered on this plot of land when it needs to be taken over by the player before use.
    /// </summary>
    public Material unownedMaterial;

    /// <summary>
    /// What renderer this plot of land has.
    /// </summary>
    public Renderer cube;

    /// <summary>
    /// Whether the player owns this land.
    /// </summary>
    public bool Owned { get => owned;}

    void Update()
    {
        if (visible)
        {
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(position.x, 0, position.y), raiseSpeed);
            //TODO: Stop lerping if you're close enough.
        }
    }

    /// <summary>
    /// Initializes this plot of land.
    /// </summary>
    /// <param name="position">Where this plot of land is located on the map.</param>
    /// <param name="visible">Whether this plot of land is visible.</param>
    /// <param name="owned">Whether the plot of land is owned by the player.</param>
    /// <param name="controller">A pointer to the controller that controls the map.</param>
    internal void Init(Vector2Int position, bool visible, bool owned, TownMapController controller)
    {
        this.position = position;
        this.owned = owned;
        this.visible = visible;
        this.controller = controller;
        this.transform.localPosition = new Vector3(position.x, -100, position.y);
        raiseSpeed += UnityEngine.Random.value / 25;
    }

    /// <summary>
    /// Causes the player to take ownership of the plot of land, also revealing all nearby land if not revealed already.
    /// </summary>
    internal void Own()
    {
        this.owned = true;
        cube.material = ownedMaterial;
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                if (!((x == -2|| x==2) && (y == 2 || y == -2)))
                {
                    controller.SetLandVisible(position.x + x, position.y + y);
                }
            }
        }
    }
}
