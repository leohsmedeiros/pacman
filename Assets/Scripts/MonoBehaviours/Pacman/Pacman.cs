using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to manage the pacman's states,
 *  behaviours and direction (according to player's inputs).
 */

[RequireComponent(typeof(PacmanAnimator))]
[RequireComponent(typeof(CharacterMovement))]
public class Pacman : MonoBehaviour {
    private PacmanAnimator _pacmanAnimator;
    private CharacterMovement _characterMovement;

    private Direction _currentDirection = Direction.RIGHT;
    private Direction currentDirection {
        set {
            _currentDirection = value;

            /*  Adapt the pacman animation with currentDirection changes */
            switch (_currentDirection) {
                case Direction.UP:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_UP);
                    break;
                case Direction.RIGHT:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_RIGHT);
                    break;
                case Direction.LEFT:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_LEFT);
                    break;
                case Direction.DOWN:
                    _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_DOWN);
                    break;
            }

        }
        get => _currentDirection;
    }

    private List<Action<Ghost>> _actionsOnGetCaughtByGhosts;

    /* Pacman only navigates between the nodes, in direction to target point */
    private Node _currentNode;

    private Direction? _inputDirection;

    private bool _isReady;

    public KeyCode keyCodeToMoveUp = KeyCode.UpArrow;
    public KeyCode keyCodeToMoveRight = KeyCode.RightArrow;
    public KeyCode keyCodeToMoveDown = KeyCode.DownArrow;
    public KeyCode keyCodeToMoveLeft = KeyCode.LeftArrow;



    private void Start() {
        _actionsOnGetCaughtByGhosts = new List<Action<Ghost>>();

        _pacmanAnimator = this.GetComponent<PacmanAnimator>();
        _characterMovement = this.GetComponent<CharacterMovement>();

        GameController.Instance.RegisterPlayer(this);

        GameController.Instance.SubscribeForGameModeChanges(gameMode => {

            if (gameMode.Equals(GameMode.INTRO) || gameMode.Equals(GameMode.DEAD)) {
                _characterMovement.Pause();
                _isReady = false;
            } else {
                _characterMovement.Resume();
                _isReady = true;
            }

        });


    }

    private Direction? OpositeDirection(Direction direction) {
        switch (direction) {
            case Direction.RIGHT: return Direction.LEFT;
            case Direction.LEFT: return Direction.RIGHT;
            case Direction.UP: return Direction.DOWN;
            case Direction.DOWN: return Direction.UP;
        }

        return null;
    }

    /*
     *  The player input is only effective when pacman is inside a node, so it will be
     *  loaded on '_inputDirection' to be used at the next node.
     *
     *  But if player pressed to go on the opposite way, will interrupt the movement
     *  and change the target node to the previous one.
     */
    private void ChangeDirection(Direction direction) {
        Direction? opositeDirection = OpositeDirection(currentDirection);

        if (opositeDirection.HasValue && direction.Equals(opositeDirection.Value)) {
            currentDirection = OpositeDirection(currentDirection).Value;
            Node node = _characterMovement.GetTargetNode().GetNeighborByDirection(currentDirection);
            _characterMovement.SetTargetNode(node);
        } else {
            _inputDirection = direction;
        }

    }

    void Update() {
        if (_isReady) {
            if (Input.GetKeyDown(keyCodeToMoveRight))
                ChangeDirection(Direction.RIGHT);

            else if (Input.GetKeyDown(keyCodeToMoveLeft))
                ChangeDirection(Direction.LEFT);

            else if (Input.GetKeyDown(keyCodeToMoveUp))
                ChangeDirection(Direction.UP);

            else if (Input.GetKeyDown(keyCodeToMoveDown))
                ChangeDirection(Direction.DOWN);


            /*
             *  Will update the target node. If there are any player input, will check
             *  if there is a neighbor in that direction and will try to move for their,
             *  otherwise will move forward to pacman's current direction.
             */

            if (_currentNode != null) {
                UpdateTargetNode();
            }
        }
    }

    /*
     *  Check if there is any input loaded and if there is a valid neighbor on
     *  this direction.
     *
     *  If there is no input, pacman will move forward.
     */
    private void UpdateTargetNode() {
        if (_inputDirection.HasValue) {

            Node nextNode = _currentNode.GetNeighborByDirection(_inputDirection.Value);

            if (nextNode != null) {
                _characterMovement.SetTargetNode(nextNode);
                currentDirection = _inputDirection.Value;
            }

            _inputDirection = null;

        } else {

            Node nextNode = _currentNode.GetNeighborByDirection(currentDirection);
            _characterMovement.SetTargetNode(nextNode);
        }
    }

    /* Reset the variables (usefull when player dies, and has enough lifes to continue) */
    public void Reset() {
        ChangeDirection(Direction.RIGHT);
        _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_RIGHT);
    }

    public Direction GetDirection() => currentDirection;


    public void SubscribeOnGetCaughtByGhosts(Action<Ghost> action) => _actionsOnGetCaughtByGhosts.Add(action);



    /*
     *  If pacman trigger with a node, will set the current node.
     *  But, if he triggers with a ghost will trigger the event of
     *  OnGetCaughtByGhosts.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _currentNode = collision.GetComponent<Node>();

        } else if (collision.tag.Equals(GameController.Instance.settings.GhostTag)) {

            _actionsOnGetCaughtByGhosts.ForEach(action => action.Invoke(collision.GetComponent<Ghost>()));
        }
    }

    /* Will set null to the current node. */
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _currentNode = null;
        }
    }

}