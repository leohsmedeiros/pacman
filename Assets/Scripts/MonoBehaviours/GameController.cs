using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";

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
    public Node previousPlayerNode { private set; get; } = null;

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


            if (_dots.Count == 0) {
                Debug.Log("Next Level");
            }

        });

        _dots.Add(dot);
    }

    public void UpdateCurrentPlayerNode (Node node) {
        if (currentPlayerNode != null)
            previousPlayerNode = currentPlayerNode;

        if (previousPlayerNode == null)
            Debug.LogWarning("previousPlayerNode is null");

        this.currentPlayerNode = node;
    }
}
