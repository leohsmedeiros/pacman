using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";
    private static readonly int DotScore = 10;



    private class GameModeSettings {
        public GameMode ModeType;
        public int Seconds;
    }


    private GameMode _currentGameMode = GameMode.SCATTER;
    public GameMode CurrentGameMode {
        private set {
            _currentGameMode = value;
            foreach(Action<GameMode> action in actionsForGameModeChange) {
                action.Invoke(_currentGameMode);
            }
        }
        get {
            return _currentGameMode;
        }
    }

    private List<Action<GameMode>> actionsForGameModeChange;

    public static GameController Instance { private set; get; }


    public static int CurrentLevel { private set; get; } = 0;
    public static int Score { private set; get; } = 0;

    private List<Dot> _dots;

    public Node CurrentPlayerNode { private set; get; } = null;
    public Direction CurrentPlayerDirection { private set; get; } = Direction.RIGHT;

    public enum GameMode { WAITING, INIT, SCATTER, CHASE, FRIGHTENED }

    private Queue<GameModeSettings> _queueGameModes;
    private bool _isFightened = false;
    private int[] timeForFrightenedModeByLevel = new int[] { 6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0 };



    public GameMode GetCurrentGameMode() {
        return _currentGameMode;
    }


    private void Awake() {
        Instance = this;
        _dots = new List<Dot>();

        _queueGameModes = new Queue<GameModeSettings>();
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 7 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 7 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 5 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 5 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 0 }); // this one is permanent

        actionsForGameModeChange = new List<Action<GameMode>>();
    }


    public void RegisterDot(Dot dot) {
        dot.SubscribeOnCaught(() => {
            _dots.Remove(dot);
            Score += DotScore;
            //Debug.Log("score: " + _score);

            if (dot.IsEnergizer) {
                Debug.Log("Caught Energizer");
            }


            if (_dots.Count == 0)
                Debug.Log("Next Level");
        });

        _dots.Add(dot);
    }

    public void RegisterPlayer(Pacman pacman) {
        pacman.SubscribeOnChangeNode((Node node) => { CurrentPlayerNode = node; });
        pacman.SubscribeOnGetCaughtByGhosts(() => Debug.Log("-1 life"));
    }

}
