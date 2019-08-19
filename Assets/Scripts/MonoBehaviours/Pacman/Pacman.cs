using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PacmanAnimator))]
[RequireComponent(typeof(PacmanMover))]
public class Pacman : MonoBehaviour {
    private PacmanAnimator _pacmanAnimator;
    private PacmanMover _directionMover;

    private List<Action<Node>> actionsOnChangeNode;
    private List<Action> actionsOnGetCaughtByGhosts;

    private void Start() {
        actionsOnChangeNode = new List<Action<Node>>();
        actionsOnGetCaughtByGhosts = new List<Action>();

        _pacmanAnimator = this.GetComponent<PacmanAnimator>();
        _directionMover = this.GetComponent<PacmanMover>();

        _directionMover.SubscribeForDirectionsChange(direction => {
            switch (direction) {
                case Direction.RIGHT:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_RIGHT);
                    break;

                case Direction.LEFT:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_LEFT);
                    break;

                case Direction.UP:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_UP);
                    break;

                case Direction.DOWN:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_DOWN);
                    break;
            }
        });

        GameController.Instance.RegisterPlayer(this);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _directionMover.ChangeDirection(Direction.RIGHT);

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            _directionMover.ChangeDirection(Direction.LEFT);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            _directionMover.ChangeDirection(Direction.UP);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            _directionMover.ChangeDirection(Direction.DOWN);
    }


    public Direction GetDirection() {
        return _directionMover.GetCurrentDirection();
    }



    public void SubscribeOnChangeNode(Action<Node> observer) {
        actionsOnChangeNode.Add(observer);
    }

    public void SubscribeOnGetCaughtByGhosts(Action observer) {
        actionsOnGetCaughtByGhosts.Add(observer);
    }




    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
 
           foreach (Action<Node> action in actionsOnChangeNode)
                action.Invoke(collision.GetComponent<Node>());

        } else if (collision.tag.Equals(GameController.GhostTag)) {

            foreach (Action action in actionsOnGetCaughtByGhosts)
                action.Invoke();

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
            foreach (Action<Node> action in actionsOnChangeNode) {
                action.Invoke(null);
            }
        }
    }

}
