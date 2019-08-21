using UnityEngine;

public class InkyGhost : Ghost {
    public Transform blincky;
    private int _tiles = 2;        

    Vector2 GetPacmanDirectionVector(Direction pacmanDirection) {
        switch (pacmanDirection) {
            case Direction.UP: return Vector2.up;
            case Direction.LEFT: return Vector2.left;
            case Direction.RIGHT: return Vector2.right;
            case Direction.DOWN: return Vector2.down;
            default: return Vector2.right;
        }
    }

    protected override Vector2 EstimateTargetPoint() {
        Vector2 pacmanPosition = _pacman.transform.position;
        Direction pacmanDirection = _pacman.GetDirection();
        Vector2 pacmanDirectionToVector2 = GetPacmanDirectionVector(pacmanDirection);

        Vector2 targetPoint = pacmanPosition + (pacmanDirectionToVector2 * _tiles);

        Vector2 blinkPosition = blincky.transform.position;

        float distance = Vector2.Distance(blinkPosition, targetPoint);
        distance *= 2;

        targetPoint.x = blinkPosition.x + distance;
        targetPoint.y = blinkPosition.y + distance;

        return targetPoint;
    }
}
