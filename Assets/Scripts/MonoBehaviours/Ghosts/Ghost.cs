using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterMovement))]
public abstract class Ghost : MonoBehaviour {
    private List<Action<Direction>> _actionsForDirectionsChange;
    private List<Action<bool>> _actionsForLifeStatusChanges;

    public float timeToBeReleased;
    private float _timer = 0;
    public AudioSource audioWhenEated;
    public GameObject scoreOnBoardTextPrefab;

    protected Pacman _pacman;

    private Direction _direction = Direction.UP;
    public Direction direction {
        private set {
            _direction = value;
            _actionsForDirectionsChange.ForEach(action => action.Invoke(_direction));
        }
        get => _direction;
    }

    /*
     *  the ghosts only navigates between the nodes, in direction to target point
     */
    protected Node _currentNode, _previousNode;

    private Vector2? _targetPoint;

    public Node scatterModeTarget;
    public Node ghostHouseDoor;

    private bool _isDead = false;
    public bool isDead {
        set {
            _isDead = value;
            _actionsForLifeStatusChanges.ForEach(action => action.Invoke(value));
        }
        get => _isDead;
    }

    /* define if the ghost can be eaten */
    public bool isFrightened { private set; get; } = false;
    private CharacterMovement _characterMovement;




    private void Awake() {
        _actionsForDirectionsChange = new List<Action<Direction>>();
        _actionsForLifeStatusChanges = new List<Action<bool>>();
        _characterMovement = this.GetComponent<CharacterMovement>();
    }

    private void Start() {
        _pacman = GameObject.FindWithTag(GameController.Instance.settings.PlayerTag).GetComponent<Pacman>();
        GameController.Instance.RegisterGhosts(this);
        GameController.Instance.SubscribeForGameModeChanges(gameMode => {
            if (!gameMode.Equals(GameMode.FRIGHTENED_FLASHING))
                isFrightened = gameMode.Equals(GameMode.FRIGHTENED);
        });
    }


    private void Update() {
        if (GameController.Instance.GetCurrentGameMode().Equals(GameMode.INTRO) ||
            GameController.Instance.GetCurrentGameMode().Equals(GameMode.DEAD))
            return;

        if (_timer > timeToBeReleased) {
            _characterMovement.pause = false;
        } else {
            _characterMovement.pause = true;
            _timer += Time.deltaTime;
        }
    }


    public void SubscribeOnDirectionsChanges(Action<Direction> action) => _actionsForDirectionsChange.Add(action);

    public void SubscribeOnLifeStatusChange(Action<bool> action) => _actionsForLifeStatusChanges.Add(action);


    /*
     *  Reset the variables (usefull when player dies, and has enough lifes to continue)
     */
    public void Reset() {
        Revive();
        _timer = 0;
        _direction = Direction.UP;
        _currentNode = null;
        _characterMovement.SetTargetNode(null);
        _previousNode = null;
    }

    /*
     *  When ghost was dead, but now is inside ghost house
     */
    public void Revive() {
        isDead = false;
        isFrightened = false;
    }

    public void GotEatenByPacman(string pointsToBeShown) {
        isDead = true;
        audioWhenEated.Play();

        GameObject scoreObject = Instantiate(scoreOnBoardTextPrefab, this.transform.position, Quaternion.identity);
        scoreObject.GetComponent<TextMesh>().text = pointsToBeShown;
        Destroy(scoreObject, GameController.Instance.settings.TimeShowingScorePointsGained);
    }


    /*
     *  particular behaviour to each ghost, so must be specialized to each one
     */
    protected abstract Vector2 EstimateTargetPoint();

    /*
     *  choose the next node based on lower distance of each neighbor node in direction on target point
     *  on frightened point, it has no target, so the ghost choose a random neighbor
     */
    private Node ChooseNextNode(Vector2? estimatedTargetPoint) {
        List<Node> neighborNodes = _currentNode.GetNeighbors();
        float distanceMin = float.MaxValue;
        Node selectedNode = neighborNodes[0];

        neighborNodes.ForEach(neighbor => {
            float distance = estimatedTargetPoint.HasValue ?
                Vector2.Distance(neighbor.GetPosition2D(), estimatedTargetPoint.Value) :
                UnityEngine.Random.Range(0f, 10f);


            if (distance < distanceMin && neighbor != _previousNode) {
                distanceMin = distance;
                selectedNode = neighbor;
                direction = _currentNode.GetDirectionByNeighborNode(neighbor);
            }
        });

        return selectedNode;
    }

    /*
     *  Will update the target point based on game state or the ghost state
     */
    private void UpdateTargetPoint() {
        if (isDead)
            _targetPoint = ghostHouseDoor.GetPosition2D();

        else if (isFrightened)
            _targetPoint = null;

        else if (GameController.Instance.GetCurrentGameMode().Equals(GameMode.SCATTER))
            _targetPoint = scatterModeTarget.GetPosition2D();

        else if (GameController.Instance.GetCurrentGameMode().Equals(GameMode.CHASE))
            _targetPoint = EstimateTargetPoint();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals(GameController.Instance.settings.NodeTag)) {
            _previousNode = _currentNode;
            _currentNode = collision.GetComponent<Node>();

            UpdateTargetPoint();

            Node targetNode = (isDead && _currentNode.Equals(ghostHouseDoor)) ?
                ghostHouseDoor.GetComponent<GhostHouseDoor>().node : ChooseNextNode(_targetPoint);

            _characterMovement.SetTargetNode(targetNode);
        }
    }


}
