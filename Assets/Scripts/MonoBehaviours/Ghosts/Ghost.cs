using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GhostMover))]
public class Ghost : MonoBehaviour {
    private GhostAi _ghostMover;

    private void Start() {
        _ghostMover = this.GetComponent<GhostAi>();
        //_ghostMover.Subs
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController
                .Instance
                .NotifyGhostCaughtPacman();
        }
    }

}
