using System;
using System.Collections;
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


    private GameMode _currentGameMode = GameMode.WAITING;
    public GameMode CurrentGameMode {
        private set {
            _currentGameMode = value;
            Debug.Log(_currentGameMode);
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

    public enum GameMode { WAITING, SCATTER, CHASE, FRIGHTENED }

    private Queue<GameModeSettings> _queueGameModes;
    private int[] _timeForFrightenedModeByLevel = new int[] { 6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0 };


    private float _timerFrightened = 0;


    public GameMode GetCurrentGameMode() {
        return _currentGameMode;
    }

    IEnumerator StartAfterSeconds() {
        yield return new WaitForSeconds(5);
        CurrentGameMode = GameMode.SCATTER;
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





    private void Start() {
        StartCoroutine(StartAfterSeconds());
    }


    private void Update() {
        if(CurrentGameMode.Equals(GameMode.FRIGHTENED)) {
            _timerFrightened += Time.deltaTime;

            float maxTime = (CurrentLevel < _timeForFrightenedModeByLevel.Length - 1) ?
                _timeForFrightenedModeByLevel[CurrentLevel] : 0;
                
            if (_timerFrightened > maxTime) {
                _timerFrightened = 0;
                CurrentGameMode = GameMode.SCATTER;
            }
        }
    }

    public void SubscribeForGameModeChanges(Action<GameMode> action) {
        actionsForGameModeChange.Add(action);
    }

    public void RegisterDot(Dot dot) {
        dot.SubscribeOnCaught(() => {
            _dots.Remove(dot);
            Score += DotScore;
            //Debug.Log("score: " + _score);

            if (dot.IsEnergizer) {
                CurrentGameMode = GameMode.FRIGHTENED;
                Debug.Log("Caught Energizer");
            }


            if (_dots.Count == 0)
                Debug.Log("Next Level");
        });

        _dots.Add(dot);
    }

    public void RegisterPlayer(Pacman pacman) {
        pacman.SubscribeOnChangeNode((Node node) => { CurrentPlayerNode = node; });
        pacman.SubscribeOnGetCaughtByGhosts((ghost) => {
            if (CurrentGameMode.Equals(GameMode.FRIGHTENED)) {
                ghost.Die();
            }else {
                Debug.Log("-1 life");
            }
        });
    }

}
