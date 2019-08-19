using UnityEngine;

public class ClydeGhost : Ghost {
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
