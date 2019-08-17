using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Node upNode;
    public Node rightNode;
    public Node leftNode;
    public Node downNode;


    public Dictionary<Direction, Node> GetNeighborsButNoOpposite(Direction direction) {
        Dictionary<Direction, Node> dictionary = new Dictionary<Direction, Node>();

        if (direction.Equals(Direction.RIGHT)) {
            dictionary.Add(Direction.UP, upNode);
            dictionary.Add(Direction.RIGHT, rightNode);
            dictionary.Add(Direction.DOWN, downNode);
        } else if (direction.Equals(Direction.LEFT)) {
            dictionary.Add(Direction.UP, upNode);
            dictionary.Add(Direction.LEFT, leftNode);
            dictionary.Add(Direction.DOWN, downNode);
        } else if (direction.Equals(Direction.UP)) {
            dictionary.Add(Direction.UP, upNode);
            dictionary.Add(Direction.RIGHT, rightNode);
            dictionary.Add(Direction.LEFT, leftNode);
        } else {
            dictionary.Add(Direction.LEFT, leftNode);
            dictionary.Add(Direction.RIGHT, rightNode);
            dictionary.Add(Direction.DOWN, downNode);
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
