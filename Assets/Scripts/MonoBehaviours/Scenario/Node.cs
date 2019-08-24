using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to configure a node
 *  and find it neighbors
 */

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

    public Direction GetDirectionByNeighborNode(Node neighborNode) {
        if (neighborNode.Equals(upNode))
            return Direction.UP;
        if (neighborNode.Equals(leftNode))
            return Direction.LEFT;
        if (neighborNode.Equals(rightNode))
            return Direction.RIGHT;
        if (neighborNode.Equals(downNode))
            return Direction.DOWN;

        return Direction.UP;
    }

    public Node GetNeighborByDirection(Direction direction) {
        if (direction.Equals(Direction.UP))
            return upNode;
        if (direction.Equals(Direction.RIGHT))
            return rightNode;
        if (direction.Equals(Direction.LEFT))
            return leftNode;
        if (direction.Equals(Direction.DOWN))
            return downNode;

        return null;
    }

    public Vector2 GetPosition2D () => new Vector2(this.transform.position.x, this.transform.position.y);

}
