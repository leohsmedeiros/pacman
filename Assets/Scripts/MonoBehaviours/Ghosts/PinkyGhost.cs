using UnityEngine;

/*
 *  In chase mode, his target is 4 tiles ahead of pacman's direction.
 */

public class PinkyGhost : Ghost {
    // four tiles ahead pacman
    private int _tiles = 4;

    protected override Vector2 EstimateTargetPoint() {   
        Direction pacmanDirection = _pacman.GetDirection();
        Vector2 pacmanPosition = _pacman.transform.position;

        switch (pacmanDirection) {
            case Direction.UP: return pacmanPosition + (Vector2.up * _tiles);
            case Direction.LEFT: return pacmanPosition + (Vector2.left * _tiles);
            case Direction.RIGHT: return pacmanPosition + (Vector2.right * _tiles);
            case Direction.DOWN: return pacmanPosition + (Vector2.down * _tiles);
            default: return pacmanPosition;
        }
    }
}
