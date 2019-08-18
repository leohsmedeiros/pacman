using System.Collections.Generic;
using UnityEngine;

public abstract class GhostAi : MonoBehaviour, IReactiveProperty<Direction> {
    private List<IObserverProperty<Direction>> observers;

    public float speed;

    protected Pacman _pacman;

    protected Direction _currentDirection = Direction.RIGHT;
    protected Node _currentNode, _targetNode, _previousNode;
    public Node scatterModeTarget;


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

    private Node ChooseNextNodeOnScatterMode() {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), scatterModeTarget.GetPosition2D());

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }


    protected abstract Node ChooseNextNode();


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Node>() != null) {
            _previousNode = _currentNode;
            _currentNode = collision.GetComponent<Node>();

            if (GameModeController.Instance.GetCurrentGameMode()
                    .Equals(GameModeController.GameMode.SCATTER))
                _targetNode = ChooseNextNodeOnScatterMode();

            else if (GameModeController.Instance.GetCurrentGameMode()
                         .Equals(GameModeController.GameMode.CHASE))
                _targetNode = ChooseNextNode();

        }
    }



    public void Subscribe(IObserverProperty<Direction> observer) {
        observers.Add(observer);
    }

    public void Unsubscribe(IObserverProperty<Direction> observer) {
        observers.Remove(observer);
    }

    public void NotifyObservers() {
        foreach (IObserverProperty<Direction> observer in observers) {
            observer.OnUpdateProperty(_currentDirection);
        }
    }

}
