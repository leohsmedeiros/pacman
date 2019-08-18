using UnityEngine;

public class PinkyGhostAi : GhostAi {
    // four tiles ahead pacman
    private int tiles = 4;

    protected override Vector2 EstimateTargetPoint() {   
        Direction pacmanDirection = _pacman.GetDirection();
        Vector2 pacmanPosition = _pacman.transform.position;

        switch (pacmanDirection) {
            case Direction.UP: return pacmanPosition + (Vector2.up * tiles);
            case Direction.LEFT: return pacmanPosition + (Vector2.left * tiles);
            case Direction.RIGHT: return pacmanPosition + (Vector2.right * tiles);
            case Direction.DOWN: return pacmanPosition + (Vector2.down * tiles);
            default: return pacmanPosition;
        }
    }
}
