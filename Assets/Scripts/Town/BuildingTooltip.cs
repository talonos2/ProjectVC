using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Attach this component to a GameObject with a collider to put a building tooltip on it.
/// 
/// This is a copy and modification of the "SimpleTooltip" class in the SimpleTooltip package.
/// 
/// TODO: Lots of copied code here from CombatTooltip. We could set up a hierarchy and move
/// most methids to a base tooltip class.
/// </summary>
[DisallowMultipleComponent]
public class BuildingTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private BuildingTooltipController tooltipController;
    private EventSystem eventSystem;
    private bool cursorInside = false;
    private bool isUIObject = false;
    private bool showing = false;

    /// <summary>
    /// A pointer to the building located on this GameObject so we don't need to call GetComponent.
    /// </summary>
    public Building building;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        tooltipController = FindObjectOfType<BuildingTooltipController>();

        // Add a new tooltip prefab if one does not exist yet
        if (!tooltipController)
        {
            tooltipController = AddTooltipPrefabToScene();
        }
        if (!tooltipController)
        {
            Debug.LogWarning("Could not find the Tooltip prefab");
        }

        if (GetComponent<RectTransform>())
            isUIObject = true;
    }

    private void Update()
    {
        if (!cursorInside)
            return;

        tooltipController.ShowTooltip();
    }

    /// <summary>
    /// Loads up the BuildingTooltip singleton.
    /// </summary>
    /// <returns></returns>
    public static BuildingTooltipController AddTooltipPrefabToScene()
    {
        return Instantiate(Resources.Load<GameObject>("BuildingTooltip")).GetComponentInChildren<BuildingTooltipController>();
    }

    /// <summary>
    /// Shows the tooltop OnPointerEnter
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isUIObject)
            return;
        ShowTooltip();
    }

    /// <summary>
    /// Hides the tooltip OnPointerExit
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isUIObject)
            return;
        HideTooltip();
    }

    /// <summary>
    /// Shows the tooltip singleton and sets its data to show the soldier's statistics.
    /// </summary>
    public void ShowTooltip()
    {
        showing = true;
        cursorInside = true;
        tooltipController.SetTooltipData(building);

        // Then tell the controller to show it
        tooltipController.ShowTooltip();
    }

    /// <summary>
    /// Hides the tooltip.
    /// </summary>
    public void HideTooltip()
    {
        if (!showing)
            return;
        showing = false;
        cursorInside = false;
        tooltipController.HideTooltip();
    }

    private void Reset()
    {
        // If UI, nothing else needs to be done
        if (GetComponent<RectTransform>())
            return;

        // If has a collider, nothing else needs to be done
        if (GetComponent<Collider>())
            return;

        // There were no colliders found when the component is added so we'll add a box collider by default
        // If you are making a 2D game you can change this to a BoxCollider2D for convenience
        // You can obviously still swap it manually in the editor but this should speed up development
        gameObject.AddComponent<BoxCollider>();
    }
}
