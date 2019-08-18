using System.Collections.Generic;
using UnityEngine;

public class InkyGhostAi : GhostAi {
    public Transform Blincky;
    private int tiles = 2;        
    
    Vector2 GetDirectionVector(Direction direction) {
        switch (direction) {
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
        Vector2 pacmanDirectionToVector2 = GetDirectionVector(pacmanDirection);

        Vector2 targetPoint = pacmanPosition + (pacmanDirectionToVector2 * tiles);

        Vector2 blinkPosition = Blincky.transform.position;

        float distance = Vector2.Distance(blinkPosition, targetPoint);
        distance *= 2;

        targetPoint.x = blinkPosition.x + distance;
        targetPoint.y = blinkPosition.y + distance;

        return targetPoint;
    }
}
