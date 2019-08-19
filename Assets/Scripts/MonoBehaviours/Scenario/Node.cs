using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Node upNode;
    public Node rightNode;
    public Node leftNode;
    public Node downNode;


    public List<Node> GetNeighbors() {
        List<Node> neighbors = new List<Node>();

        if (upNode != null)
            neighbors.Add(upNode);
        if (leftNode != null)
            neighbors.Add(leftNode);
        if (rightNode != null)
            neighbors.Add(rightNode);
        if (downNode != null)
            neighbors.Add(downNode);

        return neighbors;
    }

    public Direction GetDirectionByNode(Node node) {
        if (node.Equals(upNode))
            return Direction.UP;
        if (node.Equals(leftNode))
            return Direction.LEFT;
        if (node.Equals(rightNode))
            return Direction.RIGHT;
        if (node.Equals(downNode))
            return Direction.DOWN;

        return Direction.UP;
    }

    public Vector2 GetPosition2D () {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }
}
