using PathFind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AI used by the Aifrenburg Archer.
/// </summary>
public class ArcherAI : RangedAI
{
    public override int GetRange() { return 8; }
}
