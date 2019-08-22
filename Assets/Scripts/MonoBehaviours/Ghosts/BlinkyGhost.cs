using System.Collections.Generic;
using UnityEngine;

public class BlinkyGhost : Ghost {
    protected override Vector2 EstimateTargetPoint() => _pacman.transform.position;
}
