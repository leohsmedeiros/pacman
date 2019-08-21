using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Portal : MonoBehaviour {

    public Node destinyNode;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GlobalValues.PlayerTag) ||
            collision.tag.Equals(GlobalValues.GhostTag)) {

            collision.transform.position = destinyNode.GetPosition2D();
        }
    }
}
