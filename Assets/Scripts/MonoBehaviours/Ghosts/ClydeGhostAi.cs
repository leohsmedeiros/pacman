using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeGhostAi : GhostAi {
    protected override Vector2 EstimateTargetPoint() {
        Vector2 pacmanPosition = _pacman.transform.position;
        float distance = Vector2.Distance(_currentNode.GetPosition2D(), pacmanPosition);

        if (distance > 8) {
            return _pacman.transform.position;
        }else {
            return scatterModeTarget.GetPosition2D();
        }
    }
}
