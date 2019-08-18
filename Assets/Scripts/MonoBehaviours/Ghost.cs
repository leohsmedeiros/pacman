using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GhostMover))]
public class Ghost : MonoBehaviour {
    public enum AiMode { SCATTER, CHASE, FRIGHTENED }

    private class GhostAiModeSettings {
        public AiMode AiModeType;
        public int Seconds;
    }


    private Queue<GhostAiModeSettings> _queueChaseModes;
    private AiMode currentMode = AiMode.SCATTER;
    private GhostMover _ghostMover;

    private void Start() {
        _ghostMover = this.GetComponent<GhostMover>();
        //_ghostMover.Subs
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            GameController
                .GetInstance()
                .NotifyGhostCaughtPacman();
        }
    }

}
