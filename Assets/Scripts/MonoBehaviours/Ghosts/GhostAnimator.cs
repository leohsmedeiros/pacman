using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    void Start() {
        _ghost = this.GetComponent<Ghost>();
        _eyes = new List<GameObject> { upEye, rightEye, leftEye, downEye };

        _ghost.SubscribeOnDirectionsChanges(direction => {
            foreach(GameObject eye in _eyes) {
                eye.SetActive(false);
            }

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

        GameController.Instance.SubscribeForGameModeChanges(gameMode => {
            if (_ghost.isDead)
                return;

            if(gameMode.Equals(GameMode.FRIGHTENED)) {
                body.SetActive(false);
                eyeParent.SetActive(false);
                frightenedBody.SetActive(true);
                flashingBody.SetActive(false);
            } else if (gameMode.Equals(GameMode.FRIGHTENED_FLASHING)) {
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
