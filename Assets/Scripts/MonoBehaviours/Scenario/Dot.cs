using System;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {
    private List<Action> actionsOnGetCaught;
    public bool IsEnergizer = false;

    void Start() {
        actionsOnGetCaught = new List<Action>();

        GameController
            .Instance
            .GetComponent<GameController>()
            .RegisterDot(this);
    }

    public void SubscribeOnCaught (Action action) {
        actionsOnGetCaught.Add(action);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.PlayerTag)) {
            foreach(Action action in actionsOnGetCaught) {
                action.Invoke();
            }

            this.gameObject.SetActive(false);
        }
    }
}
