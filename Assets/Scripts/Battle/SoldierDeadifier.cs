using System;
using UnityEngine;

/// <summary>
/// Attach this component to a Soldier to make his image fade to gray and disappear. Then destroys the soldier.
/// </summary>
internal class SoldierDeadifier : MonoBehaviour
{
    private float timePassed;
    private Material materialToChange;
    private static readonly float DEATH_ANIM_DURATION = 1f;

    internal void Init(Material materialToChange)
    {
        this.materialToChange = materialToChange;
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > DEATH_ANIM_DURATION)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        materialToChange.SetFloat("_deadness", timePassed / DEATH_ANIM_DURATION);
    }
}