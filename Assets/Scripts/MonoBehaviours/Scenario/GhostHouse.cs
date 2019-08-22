using UnityEngine;

public class GhostHouse : Node {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.GhostTag)) {
            collision.GetComponent<Ghost>().Revive();
        }
    }
}
