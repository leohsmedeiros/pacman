using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PacmanAnimator))]
[RequireComponent(typeof(CharacterMovement))]
public class Pacman : MonoBehaviour {
    private PacmanAnimator _pacmanAnimator;
    private CharacterMovement _characterMovement;

    private Direction _currentDirection = Direction.RIGHT;
    private Direction currentDirection {
        set {
            _currentDirection = value;

            /*
             *  Adapt the pacman animation with currentDirection changes
             */
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

    /*
     *  Pacman only navigates between the nodes, in direction to target point
     */
    private Node _currentNode, _previousNode;


    private Direction? _inputDirection;



    private void Start() {
        _actionsOnGetCaughtByGhosts = new List<Action<Ghost>>();

        _pacmanAnimator = this.GetComponent<PacmanAnimator>();
        _characterMovement = this.GetComponent<CharacterMovement>();

        GameController.Instance.RegisterPlayer(this);
        _characterMovement.pause = false;
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
     *  and change the destiny to the previous node
     */
    void ChangeDirection(Direction direction) {
        Direction? opositeDirection = OpositeDirection(currentDirection);

        if (opositeDirection.HasValue && direction.Equals(opositeDirection.Value)) {
            currentDirection = OpositeDirection(currentDirection).Value;
            _characterMovement.SetTargetNode(_previousNode);
        } else
            _inputDirection = direction;

    }

    void Update() {
        if (GameController.Instance.GetCurrentGameMode().Equals(GameMode.INTRO))
            return;


        if (Input.GetKeyDown(KeyCode.RightArrow))
            ChangeDirection(Direction.RIGHT);

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeDirection(Direction.LEFT);

        else if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangeDirection(Direction.UP);

        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangeDirection(Direction.DOWN);


        if (_currentNode != null) {
            UpdateToNextNode();
        }

    }

    /*
     *  Check if there is any input loaded and then check if there are a neighbor
     *  of the current node on this direction.
     *
     *  If there is no input, pacman will move forward.
     */
    private void UpdateToNextNode() {
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

    /*
     *  Reset the variables (usefull when player dies, and has enough lifes to continue)
     */
    public void Reset() {
        ChangeDirection(Direction.RIGHT);
        _pacmanAnimator.SetAnimation(PacmanAnimator.PacmanAnimation.MOVE_RIGHT);
    }

    public Direction GetDirection() => currentDirection;


    public void SubscribeOnGetCaughtByGhosts(Action<Ghost> action) => _actionsOnGetCaughtByGhosts.Add(action);

    /*
     *  Will set the current node when pacman triggers with a node.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _currentNode = collision.GetComponent<Node>();

        } else if (collision.tag.Equals(GameController.Instance.settings.GhostTag)) {

            _actionsOnGetCaughtByGhosts.ForEach(action => action.Invoke(collision.GetComponent<Ghost>()));
        }
    }

    /*
     *  Will set the previous node when pacman is out of node and will set null to the current one.
     */
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _previousNode = _currentNode;
            _currentNode = null;
        }
    }

}