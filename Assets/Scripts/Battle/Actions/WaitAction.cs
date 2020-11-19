using UnityEngine;

/// <summary>
/// An action representing a soldier doing absolutely nothing.
/// </summary>
internal class WaitAction : AIAction
{
    private float timeToWait;
    private float timeThrough;
    private Soldier soldier;

    /// <summary>
    /// Creates a new wait action.
    /// </summary>
    /// <param name="timeToWait">How long to wait for</param>
    /// <param name="soldier">The soldier doing the waiting. Not currently used, might be later if we add an idle animation or something.</param>
    public WaitAction(float timeToWait, Soldier soldier)
    {
        this.timeToWait = timeToWait;
        this.soldier = soldier;
    }

    internal override bool IsComplete()
    {
        return (timeThrough > timeToWait);
    }

    internal override void DoUpdate()
    {
        timeThrough += Time.deltaTime;
    }
}