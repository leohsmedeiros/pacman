using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMover : MonoBehaviour, IReactiveProperty<Direction> {
    private List<IObserverProperty<Direction>> observers;

    public float speed;

    private Pacman _pacman;

    private Direction _currentDirection = Direction.RIGHT;
    private Node _currentNode, _targetNode, _previousNode;



    private void Awake() {
        observers = new List<IObserverProperty<Direction>>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();
    }


    private void Update() {
        if (_targetNode != null)
            this.transform.position = Vector2.MoveTowards(transform.position, _targetNode.GetPosition2D(), 4.5f * Time.deltaTime);
    }

    Node ChooseNextNode() {
        Vector2 pacmanPosition = _pacman.transform.position;
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), pacmanPosition);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Node>() != null) {
            _previousNode = _currentNode;
            _currentNode = collision.GetComponent<Node>();
            _targetNode = ChooseNextNode();
        }
    }



    public void Subscribe(IObserverProperty<Direction> observer) {
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

}
