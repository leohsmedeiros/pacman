using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalNode : Node {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.GhostTag)) {
            collision.GetComponent<Ghost>().Revive();
        }
    }
}
