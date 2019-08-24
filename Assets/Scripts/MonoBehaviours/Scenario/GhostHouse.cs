using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to triggers an event
 *  when a ghost is inside of the ghost's house.
 */

public class GhostHouse : Node {
    private List<Action<Ghost>> _actionsOnGhostIsInsideGhostHouse;

    private void Start() {
        _actionsOnGhostIsInsideGhostHouse = new List<Action<Ghost>>();
        GameController.Instance.RegisterGhostHouse(this);
    }

    public void SubscribeOnGhostIsInsideGhostHouse(Action<Ghost> action) =>
        _actionsOnGhostIsInsideGhostHouse.Add(action);

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.GhostTag)) {
            _actionsOnGhostIsInsideGhostHouse
                .ForEach(action => action.Invoke(collision.GetComponent<Ghost>()));
        }
    }
}
