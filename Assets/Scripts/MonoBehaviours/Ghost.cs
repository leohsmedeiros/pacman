using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PacmanMover))]
public class Ghost : MonoBehaviour {
    public enum AiMode { SCATTER, CHASE, FRIGHTENED }

    private class GhostAiModeSettings {
        public AiMode AiModeType;
        public int Seconds;
    }


    private Queue<GhostAiModeSettings> _queueChaseModes;
    public Node StartNode;
    private Node _currentNode, _targetNode, previousNode;
    private AiMode currentMode = AiMode.SCATTER;
    private Pacman _pacman;
    private PacmanMover _directionMover;


    IEnumerator LoopInGhostAi() {
        while (_queueChaseModes.Count > 0) {
            GhostAiModeSettings aiMode = _queueChaseModes.Dequeue();

            Debug.Log("mode type: " + aiMode.AiModeType);
            yield return new WaitForSeconds(aiMode.Seconds);
        }
    }

    void Start() {
        _pacman = GameObject.FindWithTag(GameController.PlayerTag).GetComponent<Pacman>();
        _directionMover = this.GetComponent<PacmanMover>();

        _queueChaseModes = new Queue<GhostAiModeSettings>();
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.SCATTER, Seconds = 7 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.CHASE, Seconds = 20 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.SCATTER, Seconds = 7 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.CHASE, Seconds = 20 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.SCATTER, Seconds = 5 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.CHASE, Seconds = 20 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.SCATTER, Seconds = 5 });
        _queueChaseModes.Enqueue(new GhostAiModeSettings { AiModeType = AiMode.CHASE, Seconds = 0 }); // this one is permanent

        StartCoroutine(LoopInGhostAi());
    }

    //private Node GetNextNodeFromDirectionFromAI() {
    //    if (_currentNode == null) {
    //        return null;
    //    }


    //    Vector2 pacmanPosition = _currentNode.GetPosition2D();
    //    Dictionary<Direction, Node> neighborNodes = _currentNode
    //        .GetNeighborsButNoOpposite(_currentDirection);

    //    float distance = float.MaxValue;
    //    Node selectedNode = _currentNode;

    //    foreach (KeyValuePair<Direction, Node> entry in neighborNodes) {
    //        float distanceByNeighbor = Vector2.Distance(_currentNode.GetPosition2D(), pacmanPosition);

    //        if (distanceByNeighbor < distance) {
    //            selectedNode = entry.Value;
    //            distance = distanceByNeighbor;
    //        }
    //    }

    //    return selectedNode;
    //}

    float LengthFromNode (Node targetNode) {
        Vector2 vector = targetNode.GetPosition2D() - previousNode.GetPosition2D();
        return vector.sqrMagnitude;
    }

    bool OverShotTarget() {
        float nodeToTarget = LengthFromNode(_targetNode);
        float nodeToSelf = LengthFromNode(_currentNode);

        return nodeToTarget < nodeToSelf;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Node>() != null) {
            _currentNode = collision.GetComponent<Node>();
        }
    }

}
