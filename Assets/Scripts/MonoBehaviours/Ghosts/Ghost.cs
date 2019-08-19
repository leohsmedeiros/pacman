using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour {
    private List<Action<Direction>> actionsForDirectionsChange;

    public float speed;
    public float timeToBeReleased;
    private float _timer = 0;

    protected Pacman _pacman;

    private Direction _currentDirection = Direction.RIGHT;
    public Direction CurrentDirection {
        private set {
            _currentDirection = value;
            foreach (Action<Direction> action in actionsForDirectionsChange) {
                action.Invoke(_currentDirection);
            }
        }
        get {
            return _currentDirection;
        }
    }

    protected Node _currentNode, _targetNode, _previousNode;
    public Node scatterModeTarget;


    private void Awake() {
        actionsForDirectionsChange = new List<Action<Direction>>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();
    }


    private void Update() {
        if (_timer > timeToBeReleased) {
            if (_targetNode != null)
                this.transform.position = Vector2.MoveTowards(transform.position, _targetNode.GetPosition2D(), 4.5f * Time.deltaTime);
        }else {
            _timer += Time.deltaTime;
        }
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

    private Node ChooseNextNodeOnFrightenedMode() {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMax = float.MinValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), _pacman.transform.position);

            if (distance > distanceMax && neighbor != _previousNode) {
                distanceMax = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }

    protected abstract Vector2 EstimateTargetPoint();

    private Node ChooseNextNode(Vector2 estimatedTargetPoint) {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {
            float distance = Vector2.Distance(neighbor.GetPosition2D(), estimatedTargetPoint);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
            }
        }

        return selectedNode;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.NodeTag)) {
            _previousNode = _currentNode;
            _currentNode = collision.GetComponent<Node>();

            if (GameController.Instance.GetCurrentGameMode()
                    .Equals(GameController.GameMode.SCATTER)) {

                _targetNode = ChooseNextNodeOnScatterMode();

            } else if (GameController.Instance.GetCurrentGameMode()
                          .Equals(GameController.GameMode.CHASE)) {

                Vector2 estimatedTargetPoint = EstimateTargetPoint();
                _targetNode = ChooseNextNode(estimatedTargetPoint);

            } else if (GameController.Instance.GetCurrentGameMode()
                          .Equals(GameController.GameMode.FRIGHTENED)) {

                _targetNode = ChooseNextNodeOnFrightenedMode();

            }
        }
    }



    public void SubscribeOnDirectionsChanges(Action<Direction> action) {
        actionsForDirectionsChange.Add(action);
    }


}
