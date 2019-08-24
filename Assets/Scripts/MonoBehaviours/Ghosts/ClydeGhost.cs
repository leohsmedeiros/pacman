using UnityEngine;

/*
 *  If he is farther than 8 tiles away, his targeting is identical to Blinky, using
 *  pacman current tile as his target.
 *  However, as soon as his distance to pacman becomes less than 8 tiles, he returns
 *  to the same target of scatter mode.
 */

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
