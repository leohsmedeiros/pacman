﻿using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to adapt the ghost's animation according
 *  to his state.
 */

[RequireComponent(typeof(Ghost))]
public class GhostAnimator : MonoBehaviour {

    public GameObject eyeParent;
    public GameObject frightenedBody;
    public GameObject flashingBody;
    public GameObject body;

    public GameObject upEye;
    public GameObject rightEye;
    public GameObject leftEye;
    public GameObject downEye;

    private List<GameObject> _eyes;
    private Ghost _ghost;
    public bool isFrightened { private set; get; } = false;


    /* will be observing the status changes (of game mode and of ghost) */
    void Start() {
        _ghost = this.GetComponent<Ghost>();
        _eyes = new List<GameObject> { upEye, rightEye, leftEye, downEye };

        /* Will adapt the eyes change according to ghost's direction */
        _ghost.SubscribeOnDirectionsChanges(direction => {
            _eyes.ForEach(eye => eye.SetActive(false));

            switch(direction) {
                case Direction.UP:
                    upEye.SetActive(true);
                    break;

                case Direction.RIGHT:
                    rightEye.SetActive(true);
                    break;

                case Direction.LEFT:
                    leftEye.SetActive(true);
                    break;

                case Direction.DOWN:
                    downEye.SetActive(true);
                    break;
            }
        });

        /* Will adapt the ghost's body according to his health condition */
        _ghost.SubscribeOnLifeStatusChange(isDead => {
            if (isDead) {
                body.SetActive(false);
                frightenedBody.SetActive(false);
                flashingBody.SetActive(false);
                eyeParent.SetActive(true);
            }else {
                body.SetActive(true);
                frightenedBody.SetActive(false);
                flashingBody.SetActive(false);
                eyeParent.SetActive(true);
            }
        });

        /* Will adapt the ghost's body according to GameMode */
        GameController.Instance.SubscribeForGameModeChanges(gameMode => {
            if (_ghost.isDead)
                return;

            if(gameMode.Equals(GameMode.FRIGHTENED)) {
                body.SetActive(false);
                eyeParent.SetActive(false);
                frightenedBody.SetActive(true);
                flashingBody.SetActive(false);
            } else if (gameMode.Equals(GameMode.FRIGHTENED_FLASHING) && _ghost.isFrightened) {
                body.SetActive(false);
                eyeParent.SetActive(false);
                frightenedBody.SetActive(false);
                flashingBody.SetActive(true);
            } else {
                body.SetActive(true);
                eyeParent.SetActive(true);
                frightenedBody.SetActive(false);
                flashingBody.SetActive(false);
            }
        });
    }


}
