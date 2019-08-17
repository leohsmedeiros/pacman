using System.Collections.Generic;
using UnityEngine;

public class DirectionMover : MonoBehaviour, IReactiveProperty<DirectionMover.Direction> {
    public enum Direction { RIGHT, LEFT, UP, DOWN };

    private List<IObserverProperty<Direction>> observers;

    public float speed;

    private Direction? _currentDirection  = null;

    private Node _destinyNode = null;

    private Direction? _bufferedDirectionToNextNode = null;


    private void Awake() {
        observers = new List<IObserverProperty<Direction>>();
    }



    void IReactiveProperty<Direction>.Subscribe(IObserverProperty<Direction> observer) {
        observers.Add(observer);
    }

    void IReactiveProperty<Direction>.Unsubscribe(IObserverProperty<Direction> observer) {
        observers.Remove(observer);
    }

    void IReactiveProperty<Direction>.NotifyObservers() {
        if (_currentDirection.HasValue) {
            foreach (IObserverProperty<Direction> observer in observers) {
                observer.OnUpdateProperty(_currentDirection.Value);
            }
        }
    }



    private bool IsOpositeDirection(Direction desiredDirection) {
        if (_currentDirection == null)
            return false;

        switch (_currentDirection) {
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
        Node currentNode = GameController.GetInstance().currentPlayerNode;

        if (currentNode != null) {
            if(UpdateDestinyNode(GetNextNodeFromDirection(currentNode, direction))) {
                _currentDirection = direction;
                ((IReactiveProperty<Direction>) this).NotifyObservers();
            }

        } else if (IsOpositeDirection(direction) && _destinyNode != null) {
            if (UpdateDestinyNode(GetNextNodeFromDirection(_destinyNode, direction))) {
                _currentDirection = direction;
                ((IReactiveProperty<Direction>)this).NotifyObservers();
            }

        } else {
            _bufferedDirectionToNextNode = direction;
        }

    }

    private bool UpdateDestinyNode (Node nextNode) {
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

        if (_destinyNode != null)
            this.transform.position = Vector2.MoveTowards(transform.position, _destinyNode.GetPosition2D(), speed * Time.deltaTime);


        Node currentNode = GameController.GetInstance().currentPlayerNode;

        if (currentNode != null) {
            if (_bufferedDirectionToNextNode != null) {

                if (UpdateDestinyNode(GetNextNodeFromDirection(currentNode, _bufferedDirectionToNextNode.Value))) {
                    _currentDirection = _bufferedDirectionToNextNode.Value;
                    ((IReactiveProperty<Direction>)this).NotifyObservers();

                }

                _bufferedDirectionToNextNode = null;

            }else if (_currentDirection != null) {

                UpdateDestinyNode(GetNextNodeFromDirection(currentNode, _currentDirection.Value));
            }
        }

    }
}
