using PathFind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for ranged AIs.
/// </summary>
public class RangedAI : SoldierAI
{
    public virtual int GetRange() { return 4; }

    internal override AIAction GetNextAction()
    {
        //TODO: Duplicated code from MeleeAI, with slight tweaks. Need to figure out how to generalize this. :/
        Soldier target = null;
        List<Vector2Int> enemyPositions = new List<Vector2Int>();
        foreach (Soldier enemy in manager.GetAllEnemiesOf(soldier.owner))
        {
            grid.nodes[enemy.Position.x, enemy.Position.y].occupiedBy = null;
            enemyPositions.Add(enemy.Position);
        }
        List<Vector2Int> path = Pathfinding.FindPath(grid, soldier.Position, enemyPositions);
        if (path == null)
        {
            Debug.LogError("No Path!");
            return new WaitAction(.5f, this.soldier);
        }
        foreach (Soldier enemy in manager.GetAllEnemiesOf(soldier.owner))
        {
            grid.nodes[enemy.Position.x, enemy.Position.y].occupiedBy = enemy;
        }

        target = grid.nodes[path[path.Count - 1].x, path[path.Count - 1].y].occupiedBy;
        int targetDistance = path.Count;
        Vector2Int nextStep = new Vector2Int(path[0].x, path[0].y) - soldier.Position;

        if (target != null && targetDistance <= GetRange())
        {
            return new BasicRangedAttackAction(target, soldier, GetRange());
        }

        if (target != null && targetDistance > 1)
        {
            return new MoveAction(DirectionUtils.VectorToDirection(nextStep), soldier);
        }
        return new WaitAction(.5f, this.soldier);
    }
}
