using UnityEngine;

public class Node : MonoBehaviour {
    public Node upNode;
    public Node rightNode;
    public Node leftNode;
    public Node downNode;


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
