using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";
    private static readonly int DotScore = 10;


    public static GameController Instance { private set; get; }


    private static int _currentLevel = 0;
    private static int _score = 0;

    public List<Dot> _dots;
    public List<Node> _nodes;

    public Node CurrentPlayerNode { private set; get; } = null;
    public Direction CurrentPlayerDirection { private set; get; } = Direction.RIGHT;


    private void Awake() {
        Instance = this;
        _dots = new List<Dot>();
        _nodes = new List<Node>();
    }


    public void RegisterNode(Node node) {
        _nodes.Add(node);
    }

    public void RegisterDot(Dot dot) {
        dot.SubscribeOnCaught(() => {
            _dots.Remove(dot);
            _score += DotScore;
            //Debug.Log("score: " + _score);

            if (_dots.Count == 0)
                Debug.Log("Next Level");
        });

        _dots.Add(dot);
    }


    public void UpdateCurrentPlayerNode(Node node) {
        this.CurrentPlayerNode = node;
    }

    public void NotifyGhostCaughtPacman() {
        Debug.Log("-1 life");
    }

}
