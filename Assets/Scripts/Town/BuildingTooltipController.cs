using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// This class goes on the panel in the tooltip singleton, and handles transferring
/// data from the enclosed building to the tooltip itself.
/// 
/// TODO: Some copied code here from InCombatUnitTooltip. Could consider splitting it off into a base class.
/// </summary>
public class BuildingTooltipController : MonoBehaviour
{
    /// <summary>
    /// A prototype of the storing bar. Copied once per type of inventory item this building contains.
    /// </summary>
    public Image storingBarPrototype;
    /// <summary>
    /// An array of pointers that point to the series of dots that represent the level of this building.
    /// </summary>
    public Image[] levelDots;

    //Pointers to other components on this tooltip.
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI clickText;
    public TextMeshProUGUI producesText;
    public TextMeshProUGUI consumesText;
    public TextMeshProUGUI workersText;
    public TextMeshProUGUI storageText;

    public TextMeshProUGUI storageAmountText;
    public TextMeshProUGUI workerAmountText;

    public Image workerBar;

    private Image panel;
    private RectTransform rect;
    private int showInFrames = -1;
    private bool showNow = false;
    private List<Image> storingBars;
    private int startIndex;

    //TODO: Delete this once we're sure we no longer need to trace it.
    private static int debugDestroyCount = 0;

    private void Awake()
    {
        // Load up both text layers
        var tmps = GetComponentsInChildren<TextMeshProUGUI>();

        // Keep a reference for the panel image and transform
        panel = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        // Hide at the start
        HideTooltip();

        startIndex = storageAmountText.transform.GetSiblingIndex() - 1;
        storingBarPrototype.gameObject.SetActive(false);
    }

    void Update()
    {
        ResizeToMatchText();
        UpdateShow();
    }

    private void ResizeToMatchText()
    {
        //No-op for now. We never need to resize it.
    }

    private void UpdateShow()
    {
        if (showInFrames == -1)
            return;

        if (showInFrames == 0)
            showNow = true;

        if (showNow)
        {
            rect.anchoredPosition = Input.mousePosition/1.5f;
        }

        showInFrames -= 1;
    }

    public void ShowTooltip()
    {
        // After 2 frames, showNow will be set to TRUE
        // after that the frame count wont matter
        if (showInFrames == -1)
            showInFrames = 2;
    }

    public void HideTooltip()
    {
        showInFrames = -1;
        showNow = false;
        rect.anchoredPosition = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    internal void SetTooltipData(Building building)
    {
        if (!building)
        {
            return;
        }
        for (int x = 0; x < 10; x++)
        {
            if (building.Level >= x)
            {
                levelDots[x].color = new Color(0, 1, .275f);
            }
            else
            {
                levelDots[x].color = new Color(.48f, .48f, .48f);
            }
        }
        nameText.text = building.name;
        descText.text = building.GetDesc();
        consumesText.text = building.GetConsumesText();
        producesText.text = building.GetProducesText();
        clickText.text = building.GetClickText();
        storageText.text = building.GetStorageText();
        workersText.text = building.GetWorkersText();

        workerAmountText.text = building.activeWorkers + "/" + building.MaxWorkers;
        workerBar.type = Image.Type.Filled;
        workerBar.fillMethod = Image.FillMethod.Horizontal;
        workerBar.fillAmount = (float)building.activeWorkers / (float)building.MaxWorkers;
        storageAmountText.text = building.GetStorageAmountText();

        while (storingBars.Count > building.GetTypesContainedInInventory().Count)
        {
            Image bar = storingBars[0];
            storingBars.RemoveAt(0);
            GameObject.Destroy(bar);
        }
        while (storingBars.Count < building.GetTypesContainedInInventory().Count)
        {
            Image storingBar = GameObject.Instantiate<Image>(storingBarPrototype, panel.rectTransform, false);
            storingBar.type = Image.Type.Filled;
            storingBar.fillMethod = Image.FillMethod.Horizontal;
            storingBar.transform.SetSiblingIndex(startIndex);
            storingBars.Insert(0,storingBar);
            storingBar.gameObject.SetActive(true);
        }

        int totalSoFar = 0;
        int iter = storingBars.Count - 1;

        foreach (InventoryItem item in building.GetTypesContainedInInventory())
        {
            totalSoFar += building.GetAmountInInventory(item);
            Image storingBar = storingBars[iter];
            iter -= 1;
            storingBar.fillAmount = (float)totalSoFar/(float)building.GetMaxInventoryCapacity();
            storingBar.color = item.GetColor();
        }
    }
}
