using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An action representing movement from one square to another.
/// </summary>
public class MoveAction : AIAction
{
    private Direction direction;
    private Soldier soldier;
    private float timeThrough;
    private float timeToMove;
    private bool isValid = true;

    private static readonly float BASE_TIME_TO_MOVE = .5f;

    /// <summary>
    /// Creates a new move Action
    /// </summary>
    /// <param name="direction">The direction of movement</param>
    /// <param name="soldier">The soldier doing the moving</param>
    public MoveAction(Direction direction, Soldier soldier)
    {
        this.direction = direction;
        this.soldier = soldier;
        this.timeToMove = BASE_TIME_TO_MOVE / soldier.GetModifiedSpeed();

        this.isValid = soldier.CanMoveInDirection(direction);
        if (this.isValid)
        {
            soldier.MovePositionBy(direction.Vector());
        }
    }

    internal override bool IsComplete()
    {
        return (timeThrough > timeToMove) && isValid;
    }

    internal override void DoUpdate()
    {
        timeThrough += Time.deltaTime;
        float t = (timeToMove - timeThrough)/ timeToMove;
        soldier.PlaceAtLerpedPosition(direction.Opposite(), t);
    }

}
