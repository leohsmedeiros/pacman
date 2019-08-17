using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string GameControllerTag = "GameController";
    public static readonly string PlayerTag = "Player";

    private static int _currentLevel = 0;
    public List<Dot> _dots;

    private void Awake() {
        _dots = new List<Dot>();
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
}
