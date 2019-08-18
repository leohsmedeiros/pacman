using System.Collections.Generic;
using UnityEngine;

public class BlinkyGhostAi : GhostAi {
    protected override Node ChooseNextNode() {
        Vector2 pacmanPosition = _pacman.transform.position;
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), pacmanPosition);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }
}
