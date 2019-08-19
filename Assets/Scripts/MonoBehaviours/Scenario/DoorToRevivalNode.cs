using UnityEngine;

public class DoorToRevivalNode : MonoBehaviour {
    public RevivalNode node;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.GhostTag)) {
            if (collision.GetComponent<Ghost>().IsDead)
                collision.transform.position = node.GetPosition2D();
        }
    }

}
