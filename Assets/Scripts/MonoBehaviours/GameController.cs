using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";
    private static readonly int DotScore = 10;


    private static GameController _instance;

    public static GameController GetInstance () {
        if (_instance == null) {
            _instance = GameObject
                .FindWithTag("GameController")
                .GetComponent<GameController>();
        }

        return _instance;
    }


    private static int _currentLevel = 0;
    private static int _score = 0;

    public List<Dot> _dots;
    public List<Node> _nodes;

    public Node currentPlayerNode { private set; get; } = null;

    private void Awake() {
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

    public void UpdateCurrentPlayerNode (Node node) {
        this.currentPlayerNode = node;
    }

}
