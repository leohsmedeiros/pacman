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

    public Vector2 GetPosition2D () {
        return new Vector2(this.transform.position.x, this.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController.Instance.UpdateCurrentPlayerNode(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController.Instance.UpdateCurrentPlayerNode(null);
        }
    }
}
