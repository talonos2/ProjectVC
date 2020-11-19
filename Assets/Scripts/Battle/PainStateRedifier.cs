using System;
using UnityEngine;

/// <summary>
/// Attach this to a soldier to make that soldier's portrait flash red. Removes itself when the flash is over.
/// </summary>
internal class PainStateRedifier : MonoBehaviour
{
    private float redness;
    private float timePassed;
    private Material materialToChange;
    private static readonly float FALLOFF_DURATION = .25f;

    internal void Init(float hpRatioDiff, Material materialToChange)
    {
        redness = Mathf.Sqrt(hpRatioDiff);
        this.materialToChange = materialToChange;
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > FALLOFF_DURATION)
        {
            materialToChange.SetFloat("_hurtness", redness * 0);
            GameObject.Destroy(this);
            return;
        }
        materialToChange.SetFloat("_hurtness", (redness * (FALLOFF_DURATION-timePassed / FALLOFF_DURATION))+redness);
    }
}