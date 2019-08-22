﻿using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ghost : MonoBehaviour {
    private List<Action<Direction>> _actionsForDirectionsChange;
    private List<Action<bool>> _actionsForLifeStatusChanges;

    public float speed;
    public float timeToBeReleased;
    private float _timer = 0;
    public AudioSource audioWhenEated;
    public GameObject scoreOnBoardTextPrefab;

    protected Pacman _pacman;

    private Direction _direction = Direction.UP;
    public Direction direction {
        private set {
            _direction = value;
            foreach (Action<Direction> action in _actionsForDirectionsChange) {
                action.Invoke(_direction);
            }
        }
        get => _direction;
    }

    protected Node _currentNode, _nextNode, _previousNode;

    private Vector2? _targetPoint;

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
        get => _isDead;
    }

    public bool isFrightened { private set; get; } = false;


    private void Awake() {
        _actionsForDirectionsChange = new List<Action<Direction>>();
        _actionsForLifeStatusChanges = new List<Action<bool>>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GameController.Instance.settings.PlayerTag).GetComponent<Pacman>();
        GameController.Instance.RegisterGhosts(this);
        GameController.Instance.SubscribeForGameModeChanges(gameMode => {
            if(!gameMode.Equals(GameMode.FRIGHTENED_FLASHING))
                isFrightened = gameMode.Equals(GameMode.FRIGHTENED);
        });
    }


    private void Update() {
        if (GameController.Instance.currentGameMode.Equals(GameMode.INTRO) ||
            GameController.Instance.currentGameMode.Equals(GameMode.DEAD))
            return;

        if (_timer > timeToBeReleased) {
            if (_nextNode != null) {
                this.transform.position = Vector2.MoveTowards(transform.position,
                                                              _nextNode.GetPosition2D(),
                                                              speed * Time.deltaTime);
            }
        }else {
            _timer += Time.deltaTime;
        }
    }


    public void SubscribeOnDirectionsChanges(Action<Direction> action) => _actionsForDirectionsChange.Add(action);

    public void SubscribeOnLifeStatusChange(Action<bool> action) => _actionsForLifeStatusChanges.Add(action);


    public void Reset() {
        Revive();
        _timer = 0;
        _direction = Direction.UP;
        _currentNode = null;
        _nextNode = null;
        _previousNode = null;
    }

    public void Revive() {
        isDead = false;
        isFrightened = false;
    }

    public void GotEatenByPlayer(string pointsToBeShown) {
        isDead = true;
        audioWhenEated.Play();

        GameObject scoreObject = Instantiate(scoreOnBoardTextPrefab, this.transform.position, Quaternion.identity);
        scoreObject.GetComponent<TextMesh>().text = pointsToBeShown;
        Destroy(scoreObject, GameController.Instance.settings.TimeShowingScorePointsGained);
    }

    protected abstract Vector2 EstimateTargetPoint();

    private Node ChooseNextNode(Vector2? estimatedTargetPoint) {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        foreach (Node neighbor in neighborNodes) {

            float distance = (estimatedTargetPoint == null) ?
                UnityEngine.Random.Range(0f, 10f) : Vector2.Distance(neighbor.GetPosition2D(), (Vector2) estimatedTargetPoint);

            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
                direction = _currentNode.GetDirectionByNode(neighbor);
            }
        }

        return selectedNode;
    }


    private void UpdateTargetPoint() {
        if (isDead)
            _targetPoint = ghostHouseDoor.GetPosition2D();

        else if (isFrightened)
            _targetPoint = null;

        else if (GameController.Instance.currentGameMode.Equals(GameMode.SCATTER))
            _targetPoint = scatterModeTarget.GetPosition2D();

        else if (GameController.Instance.currentGameMode.Equals(GameMode.CHASE))
            _targetPoint = EstimateTargetPoint();            
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _previousNode = _currentNode;
            _currentNode = collision.GetComponent<Node>();

            UpdateTargetPoint();

            _nextNode = (isDead && _currentNode.Equals(ghostHouseDoor)) ?
                ghostHouseDoor.GetComponent<BridgeToGhostHouse>().node : ChooseNextNode(_targetPoint);
        }
    }


}
