using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Portal : MonoBehaviour {

    public Node destinyNode;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            collision.transform.position = destinyNode.GetPosition2D();
        }
    }
}
