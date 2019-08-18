using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GhostMover))]
public class Ghost : MonoBehaviour {
    public enum AiMode { SCATTER, CHASE, FRIGHTENED }

    private class GhostAiModeSettings {
        public AiMode AiModeType;
        public int Seconds;
    }


    private Queue<GhostAiModeSettings> _queueChaseModes;
    private AiMode currentMode = AiMode.SCATTER;
    private Pacman _pacman;
    //private GhostMover _directionMover;
    private Direction _currentDirection = Direction.RIGHT;

    private Node _currentNode, _targetNode, _previousNode;

    void Start() {
        _pacman = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();
    }

    Direction OppositeCurrentDirection() {
        switch(_currentDirection) {
            case Direction.UP: return Direction.DOWN;
            case Direction.DOWN: return Direction.UP;
            case Direction.RIGHT: return Direction.LEFT;
            case Direction.LEFT: return Direction.RIGHT;
            default: return Direction.LEFT;
        }
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

        foreach(Node neighbor in neighborNodes) {
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
            //this.transform.position = _currentNode.GetPosition2D();
            _targetNode = ChooseNextNode();
        }
    }

}
