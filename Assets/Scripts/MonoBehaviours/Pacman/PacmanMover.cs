using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pacman))]
public class PacmanMover : MonoBehaviour {
    private List<Action<Direction>> _actionsForDirectionsChange;

    public float speed;

    private Direction _currentDirection = Direction.RIGHT;
    private Direction currentDirection {
        set {
            _currentDirection = value;

            foreach (Action<Direction> action in _actionsForDirectionsChange) {
                action.Invoke(_currentDirection);
            }
        }
        get => _currentDirection;
    }

    private Node _destinyNode = null;
    private Node _currentNode = null;

    /*  If player pressed some input, when pacman is not at a valid node to interact,
        will buffer this input and try again when pacman come to next node */
    private Direction? _bufferedDirectionToNextNode = null;


    private void Awake() {
        _actionsForDirectionsChange = new List<Action<Direction>>();
    }

    private void Start() {
        this.GetComponent<Pacman>().SubscribeOnChangeNode(node => _currentNode = node);
    }

    public void Reset() {
        _destinyNode = null;
        _bufferedDirectionToNextNode = null;
        _currentDirection = Direction.RIGHT;
    }

    public Direction GetDirection() => currentDirection;

    public void SubscribeForDirectionsChange(Action<Direction> action) => _actionsForDirectionsChange.Add(action);

    private bool IsOpositeDirection(Direction desiredDirection) {
        switch (currentDirection) {
            case Direction.RIGHT: return desiredDirection.Equals(Direction.LEFT);
            case Direction.LEFT: return desiredDirection.Equals(Direction.RIGHT);
            case Direction.UP: return desiredDirection.Equals(Direction.DOWN);
            case Direction.DOWN: return desiredDirection.Equals(Direction.UP);
            default: return false;
        }
    }

    // you just can go to another node if you are on some node,
    // but you can interrupt coming back to the previous node
    public void ChangeDirection(Direction direction) {
        Node currentNode = _currentNode;

        if (currentNode != null) {
            if(UpdateDestinyNode(GetNextNodeFromDirection(currentNode, direction))) {
                currentDirection = direction;
            }

        } else if (IsOpositeDirection(direction) && _destinyNode != null) {
            if (UpdateDestinyNode(GetNextNodeFromDirection(_destinyNode, direction))) {
                currentDirection = direction;
            }

        } else {
            _bufferedDirectionToNextNode = direction;
        }

    }

    public bool UpdateDestinyNode (Node nextNode) {
        if (nextNode != null) {
            _destinyNode = nextNode;
            return true;
        }

        return false;
    }

    private Node GetNextNodeFromDirection(Node node, Direction direction) {
        if (node != null) {

            if (direction.Equals(Direction.RIGHT) && node.rightNode != null)
                return node.rightNode;

            else if (direction.Equals(Direction.LEFT) && node.leftNode != null)
                return node.leftNode;

            else if (direction.Equals(Direction.UP) && node.upNode != null)
                return node.upNode;

            else if (direction.Equals(Direction.DOWN) && node.downNode != null)
                return node.downNode;

        }

        return null;
    }

    public void Update() {
        if (GameController.Instance.currentGameMode.Equals(GameMode.INTRO) ||
            GameController.Instance.currentGameMode.Equals(GameMode.DEAD))
            return;

        if (_destinyNode != null)
            this.transform.position = Vector2.MoveTowards(transform.position, _destinyNode.GetPosition2D(), speed * Time.deltaTime);


        if (_currentNode != null) {
            if (_bufferedDirectionToNextNode != null) {

                if (UpdateDestinyNode(GetNextNodeFromDirection(_currentNode, _bufferedDirectionToNextNode.Value))) {
                    currentDirection = _bufferedDirectionToNextNode.Value;
                }

                _bufferedDirectionToNextNode = null;

            } else {

                UpdateDestinyNode(GetNextNodeFromDirection(_currentNode, currentDirection));
            }
        }

    }
}
