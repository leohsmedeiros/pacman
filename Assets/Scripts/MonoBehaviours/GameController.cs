using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";

    private static readonly int DotScore = 10;
    private static readonly int EnergizerScore = 10;
    private static readonly int GhostFrightenedBaseScore = 200;
    private static readonly int GhostScoreFactor = 2;
    private static readonly float TimeToGhostsVanishOnPacmanDeath = 1.5f;
    private static readonly float TimeToStartAnimationOfPacmanDeath = 0.5f;

    private static readonly int[] _timeForFrightenedModeByLevel = { 6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0 };
    private static readonly GameModeSettings[] _gameGameModesSettingsArray = {
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 0 } // this one is permanent
    };


    public enum GameMode { WAITING, SCATTER, CHASE, FRIGHTENED, DEAD }

    private class GameModeSettings {
        public GameMode Mode;
        public int Seconds;
    }


    private GameMode _currentGameMode = GameMode.WAITING;
    public GameMode CurrentGameMode {
        private set {
            _currentGameMode = value;
            Debug.Log(_currentGameMode);
            foreach(Action<GameMode> action in _actionsForGameModeChange) {
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
    public GameObject ScoreOnBoardText;
    public float SecondsShowingScoreOnBoard = 3f;
    public Node CurrentPlayerNode { private set; get; } = null;
    public Direction CurrentPlayerDirection { private set; get; } = Direction.RIGHT;

    private List<Action<GameMode>> _actionsForGameModeChange;
    private List<Dot> _dots;
    private List<Ghost> _ghosts;

    private Pacman _pacman;
    private int _lastIndexGameModeSettings = 0;
    private float _timerFrightened = 0;
    private float _timerGameModes = 0;
    private int _factorToEatGhostsSequentially = 1;



    IEnumerator StartAfterSeconds() {
        yield return new WaitForSeconds(StarterTime);
        CurrentGameMode = _gameGameModesSettingsArray[_lastIndexGameModeSettings].Mode;
    }

    IEnumerator PacmanDieAnimation() {
        yield return new WaitForSeconds(TimeToGhostsVanishOnPacmanDeath);

        foreach (Ghost ghost in _ghosts) {
            ghost.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(TimeToStartAnimationOfPacmanDeath);

        _pacman.GetComponent<PacmanAnimator>().SetAnimation(PacmanAnimator.PacmanAnimation.DIE);
    }


    private void Awake() {
        Instance = this;

        _dots = new List<Dot>();
        _ghosts = new List<Ghost>();
        _actionsForGameModeChange = new List<Action<GameMode>>();
    }

    private void Start() {
        StartCoroutine(StartAfterSeconds());
    }

    private void Update() {
        if (CurrentGameMode.Equals(GameMode.WAITING) ||
            CurrentGameMode.Equals(GameMode.DEAD))
            return;

        if (Input.GetKey(KeyCode.Escape)) {
            CurrentGameMode = GameMode.FRIGHTENED;
            _timerFrightened = 0;
            _factorToEatGhostsSequentially = 1;
        }

        if (CurrentGameMode.Equals(GameMode.FRIGHTENED)) {
            FrightenedModeUpdate();
        }else {
            GameModeUpdate();
        }
    }


    private void FrightenedModeUpdate() {
        _timerFrightened += Time.deltaTime;

        float maxTime = (CurrentLevel < _timeForFrightenedModeByLevel.Length - 1) ?
            _timeForFrightenedModeByLevel[CurrentLevel] : 0;

        if (_timerFrightened > maxTime) {
            _timerFrightened = 0;
            _factorToEatGhostsSequentially = 1;
            CurrentGameMode = _gameGameModesSettingsArray[_lastIndexGameModeSettings].Mode;
        }
    }

    private void GameModeUpdate() {
        _timerGameModes += Time.deltaTime;

        float maxTime = (_lastIndexGameModeSettings < _gameGameModesSettingsArray.Length - 1) ?
            _gameGameModesSettingsArray[_lastIndexGameModeSettings].Seconds : float.MaxValue;

        if (_timerGameModes > maxTime) {
            _lastIndexGameModeSettings = (_lastIndexGameModeSettings + 1 < _gameGameModesSettingsArray.Length) ?
                _lastIndexGameModeSettings + 1 : _gameGameModesSettingsArray.Length - 1;

            _timerGameModes = 0;
            CurrentGameMode = _gameGameModesSettingsArray[_lastIndexGameModeSettings].Mode;
        }
    }



    public GameMode GetCurrentGameMode() {
        return _currentGameMode;
    }

    public void SubscribeForGameModeChanges(Action<GameMode> action) {
        _actionsForGameModeChange.Add(action);
    }

    public void RegisterDot(Dot dot) {
        dot.SubscribeOnCaught(() => {
            _dots.Remove(dot);

            if (dot.IsEnergizer) {
                CurrentGameMode = GameMode.FRIGHTENED;
                Score += EnergizerScore;
            } else {
                Score += DotScore;
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
                    int scoreGained = (GhostFrightenedBaseScore * _factorToEatGhostsSequentially);
                    Score += scoreGained;
                    _factorToEatGhostsSequentially *= GhostScoreFactor;

                    GameObject scoreOnBoardTextInstantiated = Instantiate(ScoreOnBoardText, ghost.transform.position, Quaternion.identity);
                    scoreOnBoardTextInstantiated.GetComponent<TextMesh>().text = scoreGained.ToString();
                    Destroy(scoreOnBoardTextInstantiated, SecondsShowingScoreOnBoard);

                } else {
                    CurrentGameMode = GameMode.DEAD;
                    StartCoroutine(PacmanDieAnimation());
                }
            }
        });
    }

}
