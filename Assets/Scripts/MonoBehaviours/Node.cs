using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Node upNode;
    public Node rightNode;
    public Node leftNode;
    public Node downNode;


    public List<Node> GetNeighbors () {
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


    public List<Direction> GetValidDirections() {
        List<Direction> neighborsDirections = new List<Direction>();

        if (upNode != null)
            neighborsDirections.Add(Direction.UP);
        if (leftNode != null)
            neighborsDirections.Add(Direction.LEFT);
        if (rightNode != null)
            neighborsDirections.Add(Direction.RIGHT);
        if (downNode != null)
            neighborsDirections.Add(Direction.DOWN);

        return neighborsDirections;
    }



    public Dictionary<Direction, Node> GetNeighborsButNoOpposite(Direction direction) {
        Dictionary<Direction, Node> dictionary = new Dictionary<Direction, Node>();

        if (direction.Equals(Direction.RIGHT)) {

            if (upNode != null) dictionary.Add(Direction.UP, upNode);
            if (rightNode != null) dictionary.Add(Direction.RIGHT, rightNode);
            if (downNode != null) dictionary.Add(Direction.DOWN, downNode);

        } else if (direction.Equals(Direction.LEFT)) {

            if (upNode != null) dictionary.Add(Direction.UP, upNode);
            if (leftNode != null) dictionary.Add(Direction.LEFT, leftNode);
            if (downNode != null) dictionary.Add(Direction.DOWN, downNode);
        
        } else if (direction.Equals(Direction.UP)) {
        
            if (upNode != null) dictionary.Add(Direction.UP, upNode);
            if (rightNode != null) dictionary.Add(Direction.RIGHT, rightNode);
            if (leftNode != null) dictionary.Add(Direction.LEFT, leftNode);
        
        } else {

            if (leftNode != null) dictionary.Add(Direction.LEFT, leftNode);
            if (rightNode != null) dictionary.Add(Direction.RIGHT, rightNode);
            if (downNode != null) dictionary.Add(Direction.DOWN, downNode);

        }

        return dictionary;
    }

    public Vector2 GetPosition2D () {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController.GetInstance().UpdateCurrentPlayerNode(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController.GetInstance().UpdateCurrentPlayerNode(null);
        }
    }
}
