using UnityEngine;

public class RevivalNode : Node {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GlobalValues.GhostTag)) {
            collision.GetComponent<Ghost>().Revive();
        }
    }
}
