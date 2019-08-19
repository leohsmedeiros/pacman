using System;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMover : MonoBehaviour {
    private List<Action<Direction>> actionsForDirectionsChange;

    public float speed;

    private Direction _currentDirection = Direction.RIGHT;
    private Direction currentDirection {
        set {
            _currentDirection = value;

            foreach (Action<Direction> action in actionsForDirectionsChange) {
                action.Invoke(_currentDirection);
            }
        }
        get {
            return _currentDirection;
        }
    }

    private Node _destinyNode = null;

    private Direction? _bufferedDirectionToNextNode = null;


    private void Awake() {
        actionsForDirectionsChange = new List<Action<Direction>>();
    }

    public Direction GetCurrentDirection() {
        return currentDirection;
    }

    public void SubscribeForDirectionsChange(Action<Direction> action) {
        actionsForDirectionsChange.Add(action);
    }


    private bool IsOpositeDirection(Direction desiredDirection) {
        switch (currentDirection) {
            case Direction.RIGHT: return desiredDirection.Equals(Direction.LEFT);
            case Direction.LEFT: return desiredDirection.Equals(Direction.RIGHT);
            case Direction.UP: return desiredDirection.Equals(Direction.DOWN);
            case Direction.DOWN: return desiredDirection.Equals(Direction.UP);
            default: return false;
        }
    }

    // rule #1: you just can go to another node if you are on some node
    // exception: you can interrupt coming back to previous node
    public void ChangeDirection(Direction direction) {
        Node currentNode = GameController.Instance.CurrentPlayerNode;

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
        if (GameController.Instance.CurrentGameMode.Equals(GameController.GameMode.WAITING))
            return;

        if (_destinyNode != null)
            this.transform.position = Vector2.MoveTowards(transform.position, _destinyNode.GetPosition2D(), speed * Time.deltaTime);


        Node currentNode = GameController.Instance.CurrentPlayerNode;

        if (currentNode != null) {
            if (_bufferedDirectionToNextNode != null) {

                if (UpdateDestinyNode(GetNextNodeFromDirection(currentNode, _bufferedDirectionToNextNode.Value))) {
                    currentDirection = _bufferedDirectionToNextNode.Value;
                }

                _bufferedDirectionToNextNode = null;

            } else {

                UpdateDestinyNode(GetNextNodeFromDirection(currentNode, currentDirection));
            }
        }

    }
}
