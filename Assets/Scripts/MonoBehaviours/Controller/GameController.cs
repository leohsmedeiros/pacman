using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private GameMode _currentGameMode = GameMode.WAITING;
    public GameMode currentGameMode {
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
    public static int Life { private set; get; } = 3;

    public float starterTime = 5f;
    public UiManager uiManager;
    public SoundManager soundManager;
    public GameObject gameOverPrefab;
    public GameObject scoreOnBoardTextPrefab;
    public float secondsShowingScoreOnBoard = 3f;
    public Node playerNode { private set; get; } = null;
    public Direction playerDirection { private set; get; } = Direction.RIGHT;

    private List<Action<GameMode>> _actionsForGameModeChange;
    private List<Dot> _dots;
    private List<Ghost> _ghosts;

    private Pacman _pacman;
    private int _lastIndexGameModeSettings = 0;
    private float _timerFrightened = 0;
    private float _timerGameModes = 0;
    private int _factorToEatGhostsSequentially = 1;



    IEnumerator StartAfterSeconds() {
        yield return new WaitForSeconds(starterTime);
        currentGameMode = GlobalValues.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
        soundManager.PlaySirenSound();
    }

    IEnumerator PacmanDieAnimation() {
        soundManager.PauseBackgroundSource();
        yield return new WaitForSeconds(GlobalValues.TimeToGhostsVanishOnPacmanDeath);

        foreach (Ghost ghost in _ghosts) {
            ghost.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(GlobalValues.TimeToStartAnimationOfPacmanDeath);

        soundManager.PlayPacManDeathSound();
        _pacman.GetComponent<PacmanAnimator>().SetAnimation(PacmanAnimator.PacmanAnimation.DIE);

        yield return new WaitForSeconds(GlobalValues.TimeToRestartSceneAfterPacmanDeath);

        LoseOneLife();
    }


    private void Awake() {
        Instance = this;

        _dots = new List<Dot>();
        _ghosts = new List<Ghost>();
        _actionsForGameModeChange = new List<Action<GameMode>>();
    }

    private void Start() {
        uiManager.UpdateLifesOnUi(Life);
        uiManager.UpdateScoreOnUi(Score);
        uiManager.UpdateHighScoreOnUi(PlayerPrefs.GetInt("highscore", 0));
        StartCoroutine(StartAfterSeconds());
    }

    private void Update() {
        if (currentGameMode.Equals(GameMode.WAITING) ||
            currentGameMode.Equals(GameMode.DEAD))
            return;

        if (currentGameMode.Equals(GameMode.FRIGHTENED)) {
            FrightenedModeUpdate();
        }else {
            GameModeUpdate();
        }
    }


    private void FrightenedModeUpdate() {
        _timerFrightened += Time.deltaTime;

        float maxTime = (CurrentLevel < GlobalValues.TimeForFrightenedModeByLevel.Length - 1) ?
            GlobalValues.TimeForFrightenedModeByLevel[CurrentLevel] : 0;

        if (_timerFrightened > maxTime) {
            _timerFrightened = 0;
            _factorToEatGhostsSequentially = 1;
            currentGameMode = GlobalValues.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
            soundManager.PlaySirenSound();
        }
    }

    private void GameModeUpdate() {
        _timerGameModes += Time.deltaTime;

        float maxTime = (_lastIndexGameModeSettings < GlobalValues.SequenceOfGameModeSettings.Length - 1) ?
            GlobalValues.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Seconds : float.MaxValue;

        if (_timerGameModes > maxTime) {
            _lastIndexGameModeSettings = (_lastIndexGameModeSettings + 1 < GlobalValues.SequenceOfGameModeSettings.Length) ?
                _lastIndexGameModeSettings + 1 : GlobalValues.SequenceOfGameModeSettings.Length - 1;

            _timerGameModes = 0;
            currentGameMode = GlobalValues.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
        }
    }

    private void NextLevel() {
        CurrentLevel++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoseOneLife() {
        Life -= 1;
        uiManager.UpdateLifesOnUi(Life);

        if (Life > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            _pacman.gameObject.SetActive(false);
            Instantiate(gameOverPrefab);

            int highscore = PlayerPrefs.GetInt("highscore");
            if (Score > highscore) {
                PlayerPrefs.SetInt("highscore", Score);
            }

        }
    }

    private void AddScore(int amount) {
        Score += amount;
        uiManager.UpdateScoreOnUi(Score);

        if (Score % GlobalValues.PointsToGainExtraLife == 0) {
            Life++;
            uiManager.UpdateLifesOnUi(Life);
            soundManager.PlayExtraLifeSound();
        }
    }


    public void RestartGame() {
        CurrentLevel = 0;
        Life = 3;
        Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                currentGameMode = GameMode.FRIGHTENED;
                soundManager.PlayFrightenedSound();
                AddScore(GlobalValues.EnergizerScore);
            } else {
                AddScore(GlobalValues.DotScore);
            }

            if (_dots.Count == 0)
                NextLevel();

        });

        _dots.Add(dot);
    }

    public void RegisterGhosts(Ghost ghost) {
        _ghosts.Add(ghost);
    }

    public void RegisterPlayer(Pacman pacman) {
        _pacman = pacman;

        _pacman.SubscribeOnChangeNode((Node node) => { playerNode = node; });
        _pacman.SubscribeOnGetCaughtByGhosts((ghost) => {
            if (!ghost.isDead) {
                if (currentGameMode.Equals(GameMode.FRIGHTENED)) {
                    int scoreGained = (GlobalValues.GhostFrightenedBaseScore * _factorToEatGhostsSequentially);

                    AddScore(scoreGained);

                    _factorToEatGhostsSequentially *= GlobalValues.GhostScoreFactor;

                    GameObject scoreOnBoardTextInstantiated = Instantiate(scoreOnBoardTextPrefab, ghost.transform.position, Quaternion.identity);
                    scoreOnBoardTextInstantiated.GetComponent<TextMesh>().text = scoreGained.ToString();
                    Destroy(scoreOnBoardTextInstantiated, secondsShowingScoreOnBoard);

                } else {
                    currentGameMode = GameMode.DEAD;
                    StartCoroutine(PacmanDieAnimation());
                }
            }
        });
    }

}
