using PathFind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for melee based AIs.
/// </summary>
public class MeleeAI : SoldierAI
{
    internal override AIAction GetNextAction()
    {
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

        if (target != null && targetDistance == 1f)
        {
            return new BasicMeleeAttackAction(target, soldier);
        }

        if (target != null && targetDistance > 1)
        {
            return new MoveAction(DirectionUtils.VectorToDirection(nextStep), soldier);
        }
        return new WaitAction(.5f, this.soldier);
    }
}
