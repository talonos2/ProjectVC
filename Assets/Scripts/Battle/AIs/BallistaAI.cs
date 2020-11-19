using PathFind;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The AI used by the Aifrenburg Ballista.
/// </summary>
public class BallistaAI : RangedAI
{
    public override int GetRange() { return 16; }
}
