using System;
using System.Collections.Generic;
using PathFind;
using UnityEngine;

/// <summary>
/// Represents the algoritm by which any Soldier decides what action to take next.
/// </summary>
public class SoldierAI
{
    protected Soldier soldier;
    protected ChessGrid grid;
    protected SoldierStats stats;
    protected AutoChessManager manager;

    internal void SetControlledPawn(Soldier soldier)
    {
        this.soldier = soldier;
    }

    /// <summary>
    /// Gets the next action this AI will take.
    /// </summary>
    /// <returns>An action, an object that handles the update loop for whatever the soldier does next.</returns>
    internal virtual AIAction GetNextAction()
    {
        return new WaitAction(.5f, this.soldier);
    }

    internal void SetStats(SoldierStats stats)
    {
        this.stats = stats;
    }

    internal void SetGrid(ChessGrid grid)
    {
        this.grid = grid;
    }

    internal void SetManager(AutoChessManager manager)
    {
        this.manager = manager;
    }
}