using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// This class goes on the panel in the tooltip singleton, and handles transferring
/// data from the enclosed soldier to the tooltip itself.
/// </summary>
public class InCombatUnitTooltipController : MonoBehaviour
{
    /// <summary>
    /// A pointer to the stat boxes in this tooltip.
    /// </summary>
    public TextMeshProUGUI[] statBoxes = new TextMeshProUGUI[0];

    /// <summary>
    /// A pointer to the name box in this tooltip.
    /// </summary>
    public TextMeshProUGUI nameText;

    /// <summary>
    /// A pointer to the class box in this tooltip.
    /// </summary>
    public TextMeshProUGUI classText;

    /// <summary>
    /// A pointer to the level box in this tooltip.
    /// </summary>
    public TextMeshProUGUI lvlText;

    private Image panel;
    private RectTransform rect;
    private int showInFrames = -1;
    private bool showNow = false;
    private Soldier soldier;

    private void Awake()
    {
        // Keep a reference for the panel image and transform
        panel = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        // Hide at the start
        HideTooltip();
    }

    void Update()
    {
        ResizeToMatchText();
        UpdateShow();
        SetTooltipData(soldier);
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
            rect.anchoredPosition = Input.mousePosition;
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

    /// <summary>
    /// Shunts the tooltip singleton off the screen, ready to be reused later.
    /// </summary>
    public void HideTooltip()
    {
        showInFrames = -1;
        showNow = false;
        rect.anchoredPosition = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
    }

    /// <summary>
    /// Given a soldier, transfer its attributes into the tooltip.
    /// </summary>
    /// <param name="soldier">The soldier to represent with the tooltip.</param>
    internal void SetTooltipData(Soldier soldier)
    {
        if (!soldier)
        {
            return;
        }
        this.soldier = soldier;
        statBoxes[0].text = "HP: " + soldier.currentHP + " / " + soldier.stats.Hp;
        statBoxes[1].text = "MP: " + soldier.currentMP + " / " + soldier.stats.Mp;
        statBoxes[2].text = "Atk: " + soldier.stats.Att;
        statBoxes[3].text = "Def: " + soldier.stats.Def;
        statBoxes[4].text = "Mag: " + soldier.stats.Mag;
        statBoxes[5].text = "Res: " + soldier.stats.Res;
        statBoxes[6].text = "Agi: " + soldier.stats.Agi;
        statBoxes[7].text = "Spd: " + soldier.stats.Spd;
        nameText.text = soldier.stats.name;
        classText.text = "The "+soldier.stats.GetClassName();
        lvlText.text = "Lv. "+ soldier.stats.Level;
    }
}
