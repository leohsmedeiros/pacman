using UnityEngine;

/*
 *  In chase mode, his target is pacman's position.
 */

public class BlinkyGhost : Ghost {
    protected override Vector2 EstimateTargetPoint() => _pacman.transform.position;
}
