using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMover : MonoBehaviour, IReactiveProperty<Direction> {
    private List<IObserverProperty<Direction>> observers;

    public float speed;

    private Direction _currentDirection = Direction.RIGHT;

    private Node _destinyNode = null;

    private Pacman _player;

    private Node _currentNode, _targetNode, _previousNode;



    private void Awake() {
        observers = new List<IObserverProperty<Direction>>();
    }

    private void Start() {
        _player = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();
    }



    public Direction GetCurrentDirection() {
        return _currentDirection;
    }

    void IReactiveProperty<Direction>.Subscribe(IObserverProperty<Direction> observer) {
        observers.Add(observer);
    }

    void IReactiveProperty<Direction>.Unsubscribe(IObserverProperty<Direction> observer) {
        observers.Remove(observer);
    }

    void IReactiveProperty<Direction>.NotifyObservers() {
        foreach (IObserverProperty<Direction> observer in observers) {
            observer.OnUpdateProperty(_currentDirection);
        }
    }



    private Node GetNextNode() {
        Vector2 pacmanPosition = _player.transform.position;
        Debug.Log("pacmanPosition: " + pacmanPosition);

        float distanceMin = float.MaxValue;
        Node selectedNode = null;

        List<Node> possibleNodes = _currentNode.GetNeighbors();
        Debug.Log("possibleNodes: " + possibleNodes.Count);

        foreach (Node node in possibleNodes) {
            float distance = GetDistance(node.GetPosition2D(), pacmanPosition);
            if (distance < distanceMin) {
                distanceMin = distance;
                selectedNode = _currentNode.upNode;
            }
        }

        return selectedNode;

    }



    public bool UpdateDestinyNode(Node nextNode) {
        Debug.Log("selected node: " + nextNode.name);
        if (nextNode != null) {
            _destinyNode = nextNode;
            _previousNode = _currentNode;
            return true;
        }

        return false;
    }



    public void Update() {

        if (_destinyNode != null) {
            this.transform.position = Vector2.MoveTowards(transform.position, _destinyNode.GetPosition2D(), speed * Time.deltaTime);
        }

        if (_currentNode != null) {
            UpdateDestinyNode(GetNextNode());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Node>() != null) {
            _currentNode = collision.GetComponent<Node>();

            if (_currentNode != null) {
                UpdateDestinyNode(GetNextNode());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Node>() != null) {
            _currentNode = null; // collision.GetComponent<Node>();
        }
    }

    private float GetDistance (Vector2 posA, Vector2 posB) {
        float dx = posA.x - posB.x;
        float dy = posA.y - posB.y;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
