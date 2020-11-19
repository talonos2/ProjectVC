using System;
using UnityEngine;

public class AIAction
{
    /// <summary>
    /// Is the action complete? AIs will not pick a new action until their old action is complete.
    /// </summary>
    /// <returns>Whether the action is complete.</returns>
    internal virtual bool IsComplete()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates the action. Actions are not a monobehaviors, so the update method is called
    /// externally (by the AI of the soldier that is doing the hitting.)
    /// </summary>
    internal virtual void DoUpdate()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a hitsplat. In the base class because lots of actions use it.
    /// </summary>
    /// <param name="info"></param>
    protected void CreateHitsplat(HitsplatInfo info)
    {
        Debug.Log(info.damage+", "+info.type);
    }
}