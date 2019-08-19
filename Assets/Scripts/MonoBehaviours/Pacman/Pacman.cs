using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PacmanAnimator))]
[RequireComponent(typeof(PacmanMover))]
public class Pacman : MonoBehaviour {
    private PacmanAnimator _pacmanAnimator;
    private PacmanMover _directionMover;

    private List<Action<Node>> actionsOnChangeNode;
    private List<Action<Ghost>> actionsOnGetCaughtByGhosts;

    private void Start() {
        actionsOnChangeNode = new List<Action<Node>>();
        actionsOnGetCaughtByGhosts = new List<Action<Ghost>>();

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
        if (GameController.Instance.CurrentGameMode.Equals(GameController.GameMode.WAITING))
            return;


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



    public void SubscribeOnChangeNode(Action<Node> action) {
        actionsOnChangeNode.Add(action);
    }

    public void SubscribeOnGetCaughtByGhosts(Action<Ghost> action) {
        actionsOnGetCaughtByGhosts.Add(action);
    }




    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
 
           foreach (Action<Node> action in actionsOnChangeNode)
                action.Invoke(collision.GetComponent<Node>());

        } else if (collision.tag.Equals(GameController.GhostTag)) {

            foreach (Action<Ghost> action in actionsOnGetCaughtByGhosts)
                action.Invoke(collision.GetComponent<Ghost>());

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
