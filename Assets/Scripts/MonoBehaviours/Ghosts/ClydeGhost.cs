using UnityEngine;

public class ClydeGhost : Ghost {
    private int _tiles = 8;

    protected override Vector2 EstimateTargetPoint() {
        Vector2 pacmanPosition = _pacman.transform.position;
        float distance = Vector2.Distance(_currentNode.GetPosition2D(), pacmanPosition);

        if (distance > _tiles) {
            return _pacman.transform.position;
        } else {
            return scatterModeTarget.GetPosition2D();
        }
    }
}
