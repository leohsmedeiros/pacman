using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour {
    private List<Action<Direction>> actionsForDirectionsChange;
    private List<Action<bool>> actionsForLifeStatusChanges;

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
    public Node ghostHouseDoor;

    private bool _isDead = false;
    public bool IsDead { 
        set {
            _isDead = value;
            foreach (Action<bool> action in actionsForLifeStatusChanges) {
                action.Invoke(value);
            }
        }
        get {
            return _isDead;
        }
    }



    private void Awake() {
        actionsForDirectionsChange = new List<Action<Direction>>();
        actionsForLifeStatusChanges = new List<Action<bool>>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();

        GameController.Instance.SubscribeForGameModeChanges(ActionsByGameMode);
        GameController.Instance.RegisterGhosts(this);
    }


    private void Update() {
        if (GameController.Instance.CurrentGameMode.Equals(GameController.GameMode.WAITING) ||
            GameController.Instance.CurrentGameMode.Equals(GameController.GameMode.DEAD))
            return;

        if (_timer > timeToBeReleased) {
            if (_targetNode != null)
                this.transform.position = Vector2.MoveTowards(transform.position, _targetNode.GetPosition2D(), 4.5f * Time.deltaTime);
        }else {
            _timer += Time.deltaTime;
        }
    }



    public void SubscribeOnDirectionsChanges(Action<Direction> action) {
        actionsForDirectionsChange.Add(action);
    }

    public void SubscribeOnLifeStatusChange(Action<bool> action) {
        actionsForLifeStatusChanges.Add(action);
    }

    private void Die() {
        IsDead = true;
    }

    public void Revive() {
        IsDead = false;
    }

    protected abstract Vector2 EstimateTargetPoint();

    private Node ChooseNextNode(Vector2 estimatedTargetPoint, bool isFrightened) {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {

            float distance = (isFrightened) ?
                UnityEngine.Random.Range(0f, 10f) : Vector2.Distance(neighbor.GetPosition2D(), estimatedTargetPoint);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
                CurrentDirection = _currentNode.GetDirectionByNode(neighbor);
            }
        }

        return selectedNode;
    }


    private void ActionsByGameMode(GameController.GameMode gameMode) {
        if (IsDead) {

            _targetNode = ChooseNextNode(ghostHouseDoor.GetPosition2D(), false);

        } else if (gameMode.Equals(GameController.GameMode.SCATTER)) {

            _targetNode = ChooseNextNode(scatterModeTarget.GetPosition2D(), false);

        } else if (gameMode.Equals(GameController.GameMode.CHASE)) {

            Vector2 estimatedTargetPoint = EstimateTargetPoint();
            _targetNode = ChooseNextNode(estimatedTargetPoint, false);

        } else if (gameMode.Equals(GameController.GameMode.FRIGHTENED)) {

            _targetNode = ChooseNextNode(_pacman.transform.position, true);

        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (IsDead) {

            if (collision.GetComponent<BridgeToRevivalNode>() != null) {
                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();

                _targetNode = collision.GetComponent<BridgeToRevivalNode>().node;
            } else if (collision.tag.Equals(GameController.NodeTag)) {
                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();
                ActionsByGameMode(GameController.Instance.CurrentGameMode);
            }

        } else {

            if (collision.tag.Equals(GameController.NodeTag)) {

                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();
                ActionsByGameMode(GameController.Instance.CurrentGameMode);

            } else if (collision.tag.Equals(GameController.PlayerTag) &&
                       GameController.Instance.CurrentGameMode.Equals(GameController.GameMode.FRIGHTENED)) {

                Die();

            }

        }
    }


}
