using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ghost : MonoBehaviour {
    private List<Action<Direction>> _actionsForDirectionsChange;
    private List<Action<bool>> _actionsForLifeStatusChanges;

    public float speed;
    public float timeToBeReleased;
    private float _timer = 0;
    public AudioSource audioWhenEated;

    protected Pacman _pacman;

    private Direction _direction = Direction.RIGHT;
    public Direction direction {
        private set {
            _direction = value;

            foreach (Action<Direction> action in _actionsForDirectionsChange) {
                action.Invoke(_direction);
            }
        }
        get {
            return _direction;
        }
    }

    protected Node _currentNode, _targetNode, _previousNode;
    public Node scatterModeTarget;
    public Node ghostHouseDoor;

    private bool _isDead = false;
    public bool isDead { 
        set {
            _isDead = value;
            foreach (Action<bool> action in _actionsForLifeStatusChanges) {
                action.Invoke(value);
            }
        }
        get {
            return _isDead;
        }
    }



    private void Awake() {
        _actionsForDirectionsChange = new List<Action<Direction>>();
        _actionsForLifeStatusChanges = new List<Action<bool>>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GlobalValues.PlayerTag).GetComponent<Pacman>();

        GameController.Instance.SubscribeForGameModeChanges(ActionsByGameMode);
        GameController.Instance.RegisterGhosts(this);
    }


    private void Update() {
        if (GameController.Instance.currentGameMode.Equals(GameMode.WAITING) ||
            GameController.Instance.currentGameMode.Equals(GameMode.DEAD))
            return;

        if (_timer > timeToBeReleased) {
            if (_targetNode != null)
                this.transform.position = Vector2.MoveTowards(transform.position, _targetNode.GetPosition2D(), speed * Time.deltaTime);
        }else {
            _timer += Time.deltaTime;
        }
    }



    public void SubscribeOnDirectionsChanges(Action<Direction> action) {
        _actionsForDirectionsChange.Add(action);
    }

    public void SubscribeOnLifeStatusChange(Action<bool> action) {
        _actionsForLifeStatusChanges.Add(action);
    }

    public void Revive() {
        isDead = false;
    }

    private void Die() {
        isDead = true;
        audioWhenEated.Play();
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
                direction = _currentNode.GetDirectionByNode(neighbor);
            }
        }

        return selectedNode;
    }


    private void ActionsByGameMode(GameMode gameMode) {
        if (isDead) {

            _targetNode = ChooseNextNode(ghostHouseDoor.GetPosition2D(), false);

        } else if (gameMode.Equals(GameMode.SCATTER)) {

            _targetNode = ChooseNextNode(scatterModeTarget.GetPosition2D(), false);

        } else if (gameMode.Equals(GameMode.CHASE)) {

            Vector2 estimatedTargetPoint = EstimateTargetPoint();
            _targetNode = ChooseNextNode(estimatedTargetPoint, false);

        } else if (gameMode.Equals(GameMode.FRIGHTENED)) {

            _targetNode = ChooseNextNode(_pacman.transform.position, true);

        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {

        if (isDead) {

            if (collision.GetComponent<BridgeToRevivalNode>() != null) {
                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();

                _targetNode = collision.GetComponent<BridgeToRevivalNode>().node;
            } else if (collision.tag.Equals(GlobalValues.NodeTag)) {
                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();
                ActionsByGameMode(GameController.Instance.currentGameMode);
            }

        } else {

            if (collision.tag.Equals(GlobalValues.NodeTag)) {

                _previousNode = _currentNode;
                _currentNode = collision.GetComponent<Node>();
                ActionsByGameMode(GameController.Instance.currentGameMode);

            } else if (collision.tag.Equals(GlobalValues.PlayerTag) &&
                       GameController.Instance.currentGameMode.Equals(GameMode.FRIGHTENED)) {

                Die();

            }

        }
    }


}
