using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages displaying and animating a hitsplat.
/// </summary>
public class Hitsplat : MonoBehaviour
{
    /// <summary>
    /// How long until the hitsplat begins to fade.
    /// </summary>
    public float timeUntilStartDisappear;

    /// <summary>
    /// How long until the hitsplat finishes fading.
    /// </summary>
    public float timeOfDisappear;

    /// <summary>
    /// How high the hitsplat gets before disappearing.
    /// </summary>
    public float height;

    private Camera cam;
    private float timePassed;
    private TextMeshPro text;

    internal void Init(HitsplatInfo hitsplatInfo, Soldier soldier)
    {
        this.text = this.GetComponent<TextMeshPro>();
        this.transform.SetParent(soldier.transform);
        this.transform.localPosition = new Vector3(0, .5f, -.15f);
        text.text = ""+hitsplatInfo.damage;
        if (hitsplatInfo.result.IsCrit())
        {
            text.text += "\nCrit!";
            text.color = new Color32(255, 107, 107,255);
        }
        else if (hitsplatInfo.result==AttackResult.MISS)
        {
            text.color = new Color32(128, 128, 120,255);
            text.text = "Miss";
        }
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeUntilStartDisappear)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - (timePassed - timeUntilStartDisappear) / (timeOfDisappear - timeUntilStartDisappear));
        }
        if (timePassed > timeOfDisappear)
        {
            GameObject.Destroy(this.gameObject);
        }
        this.transform.localPosition = new Vector3(0, .5f, -.15f-(timePassed/timeUntilStartDisappear)*height);
        transform.LookAt(cam.transform);
    }
}