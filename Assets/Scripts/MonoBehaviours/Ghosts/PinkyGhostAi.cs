using System.Collections.Generic;
using UnityEngine;

public class PinkyGhostAi : GhostAi {

    private Vector2 EstimateTargetPoint(Direction pacmanDirection, Vector2 pacmanPosition) {
        switch (pacmanDirection) {
            case Direction.UP: return pacmanPosition + (Vector2.up * 3);
            case Direction.LEFT: return pacmanPosition + (Vector2.left * 3);
            case Direction.RIGHT: return pacmanPosition + (Vector2.right * 3);
            case Direction.DOWN: return pacmanPosition + (Vector2.down * 3);
            default: return pacmanPosition;
        }
    }

    protected override Node ChooseNextNode() {   
        Direction pacmanDirection = _pacman.GetDirection();
        Vector2 targetPoint = EstimateTargetPoint(pacmanDirection, _pacman.transform.position);

        List <Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), targetPoint);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }
}
