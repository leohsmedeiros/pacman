using UnityEngine;

/*
 *  The responsibility of this script is to configure a portal
 *  with the target node and repositioning to that target when
 *  a character is inside of trigger.
 *
 *  PS: Must be a node, otherwise would not be accessible.
 */

[RequireComponent(typeof(Node))]
[RequireComponent(typeof(CircleCollider2D))]
public class Portal : MonoBehaviour {

    public Node destinyNode;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.PlayerTag) ||
            collision.tag.Equals(GameController.Instance.settings.GhostTag)) {

            collision.transform.position = destinyNode.GetPosition2D();
        }
    }
}
