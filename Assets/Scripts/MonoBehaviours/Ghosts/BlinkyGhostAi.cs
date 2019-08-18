using System.Collections.Generic;
using UnityEngine;

public class BlinkyGhostAi : GhostAi {
    protected override Vector2 EstimateTargetPoint() {
        return _pacman.transform.position;
    }
}
