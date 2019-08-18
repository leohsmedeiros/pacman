using System;
using System.Threading;
using UnityEngine;

public class Dot : MonoBehaviour {
    private ThreadStart onCaught = null;

    void Start() {
        GameController
            .Instance
            .GetComponent<GameController>()
            .RegisterDot(this);
    }

    public void SubscribeOnCaught (ThreadStart threadStart) {
        this.onCaught = threadStart;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            if (onCaught != null) {
                new Thread(onCaught).Start();
                this.gameObject.SetActive(false);
            }
        }
    }
}
