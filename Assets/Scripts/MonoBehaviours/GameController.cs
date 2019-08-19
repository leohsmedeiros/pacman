using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";

    private static readonly int DotScore = 10;



    public enum GameMode { WAITING, SCATTER, CHASE, FRIGHTENED, DEAD }

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

    public static GameController Instance { private set; get; }
    public static int CurrentLevel { private set; get; } = 0;
    public static int Score { private set; get; } = 0;

    public float StarterTime = 5f;
    public Node CurrentPlayerNode { private set; get; } = null;
    public Direction CurrentPlayerDirection { private set; get; } = Direction.RIGHT;

    private List<Action<GameMode>> actionsForGameModeChange;
    private List<Dot> _dots;
    private List<Ghost> _ghosts;
    private Pacman _pacman;
    private Queue<GameModeSettings> _queueGameModes;
    private int[] _timeForFrightenedModeByLevel = new int[] { 6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0 };


    private float _timerFrightened = 0;



    IEnumerator StartAfterSeconds() {
        yield return new WaitForSeconds(StarterTime);
        CurrentGameMode = GameMode.SCATTER;
    }

    IEnumerator DieAnimation() {
        yield return new WaitForSeconds(1.5f);

        foreach (Ghost ghost in _ghosts) {
            ghost.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        _pacman.GetComponent<PacmanAnimator>().SetAnimation(PacmanAnimator.PacmanAnimation.DIE);
    }


    private void Awake() {
        Instance = this;
        _dots = new List<Dot>();
        _ghosts = new List<Ghost>();

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




    public GameMode GetCurrentGameMode() {
        return _currentGameMode;
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

    public void RegisterGhosts(Ghost ghost) {
        _ghosts.Add(ghost);
    }

    public void RegisterPlayer(Pacman pacman) {
        _pacman = pacman;

        _pacman.SubscribeOnChangeNode((Node node) => { CurrentPlayerNode = node; });
        _pacman.SubscribeOnGetCaughtByGhosts((ghost) => {
            if (!ghost.IsDead) {
                if (CurrentGameMode.Equals(GameMode.FRIGHTENED)) {
                    Score += 100;
                } else {
                    CurrentGameMode = GameMode.DEAD;
                    StartCoroutine(DieAnimation());
                }
            }
        });
    }

}
