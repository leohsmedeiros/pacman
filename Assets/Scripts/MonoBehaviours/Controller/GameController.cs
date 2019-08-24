using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FruitManager))]
[RequireComponent(typeof(UiManager))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(GameModeManager))]
[RequireComponent(typeof(SceneChanger))]
public class GameController : MonoBehaviour {

    public static GameController Instance { private set; get; }
    public static int Level { private set; get; } = 0;
    public static int Score { private set; get; } = 0;
    public static int Life { private set; get; } = 3;
    private static int _factorToGainExtraLife = 1;

    public Settings settings;
    public Node starterNode;
    public Node ghostsHouse;

    private List<Dot> _dots;
    private List<Ghost> _ghosts;
    private int _eatenDotsAmount = 0;

    private Pacman _pacman;
    private SceneChanger _sceneChanger;
    private StageSettings _stageSettings;
    private FruitManager _fruitManager;
    private UiManager _uiManager;
    private SoundManager _soundManager;
    private GameModeManager _gameModeManager;


    IEnumerator PacmanDieAnimation() {
        _soundManager.PauseBackgroundSource();
        yield return new WaitForSeconds(settings.TimeToGhostsVanishOnPacmanDeath);

        _ghosts.ForEach(ghost => ghost.gameObject.SetActive(false));

        yield return new WaitForSeconds(settings.TimeToStartAnimationOfPacmanDeath);

        _soundManager.PlayPacManDeathSound();
        _pacman.GetComponent<PacmanAnimator>().SetAnimation(PacmanAnimator.PacmanAnimation.DIE);

        yield return new WaitForSeconds(settings.TimeToRestartSceneAfterPacmanDeath);

        LoseOneLife();
    }


    private void Awake() {
        Instance = this;
        _sceneChanger = this.GetComponent<SceneChanger>();

        _fruitManager = this.GetComponent<FruitManager>();
        _uiManager = this.GetComponent<UiManager>();
        _soundManager = this.GetComponent<SoundManager>();
        _gameModeManager = this.GetComponent<GameModeManager>();

        _dots = new List<Dot>();
        _ghosts = new List<Ghost>();

        if (Level < settings.SequenceOfStageSettings.Length) {
            _stageSettings = settings.SequenceOfStageSettings[Level];
        } else {
            int lastIndex = settings.SequenceOfStageSettings.Length - 1;
            _stageSettings = settings.SequenceOfStageSettings[lastIndex];
        }

        _gameModeManager.stageSettings = _stageSettings;  

        Fruit fruit = _fruitManager.GetFruitByType(_stageSettings.fruitType);
        _uiManager.FruitOnGUI(fruit.GetSprite());
        _uiManager.LifesOnGUI(Life);
        _uiManager.ScoreOnGUI(Score);
        _uiManager.HighScoreOnGUI(PlayerPrefs.GetInt(settings.HighScoreKeyPlayerPrefs, 0));
    }

    private void Start() {
        _gameModeManager.SubscribeForGameModeChanges(gameMode => {
            switch (gameMode) {
                case GameMode.INTRO:
                    _soundManager.PlayIntroSound();
                    break;

                case GameMode.FRIGHTENED:
                    _soundManager.PlayFrightenedSound();
                    break;

                case GameMode.SCATTER:
                case GameMode.CHASE:
                    _soundManager.PlaySirenSound();
                    break;
            }
        });

        _gameModeManager.currentGameMode = GameMode.INTRO;
    }


    private void NextLevel() {
        Level++;
        _sceneChanger.LoadLevel();
    }

    private void LoseOneLife() {
        Life -= 1;
        _uiManager.LifesOnGUI(Life);

        if (Life > 0) {
            _ghosts.ForEach(ghost => {
                ghost.transform.position = ghostsHouse.GetPosition2D();
                ghost.gameObject.SetActive(true);
                ghost.Reset();
            });

            _pacman.transform.position = starterNode.GetPosition2D();
            _pacman.Reset();

            _gameModeManager.Reset();

        } else {
            _pacman.gameObject.SetActive(false);

            int highscore = PlayerPrefs.GetInt(settings.HighScoreKeyPlayerPrefs);
            if (Score > highscore) {
                PlayerPrefs.SetInt(settings.HighScoreKeyPlayerPrefs, Score);
                _uiManager.HighScoreOnGUI(Score);
            }

            _gameModeManager.GameOver();
        }
    }

    private void AddScore(int amount) {
        Score += amount;
        _uiManager.ScoreOnGUI(Score);

        if (Score > _factorToGainExtraLife * settings.PointsToGainExtraLife) {
            Life++;
            _factorToGainExtraLife++;
            _uiManager.LifesOnGUI(Life);
            _soundManager.PlayExtraLifeSound();
        }
    }

    private void OnDotGetCaught(Dot dot) {
        _dots.Remove(dot);

        if (dot.IsEnergizer) {
            _gameModeManager.currentGameMode = GameMode.FRIGHTENED;
            AddScore(settings.EnergizerScore);
        } else {
            _eatenDotsAmount++;
            AddScore(settings.DotScore);

            if (_eatenDotsAmount.Equals(settings.DotsAmountToShowFruitFirstTime) ||
                _eatenDotsAmount.Equals(settings.DotsAmountToShowFruitSecondTime)) {

                _fruitManager.InstantiateFruit(_stageSettings.fruitType);
            } else {
                _fruitManager.DestroyFruit();
            }
        }

        if (_dots.Count == 0)
            NextLevel();
    }

    private void OnPacmanGetCaughtByGhosts(Ghost ghost) {
        if (!ghost.isDead) {
            if (ghost.isFrightened) {
                int scoreGained =
                    settings.GhostFrightenedBaseScore * _gameModeManager.factorToEatGhostsSequentially;

                AddScore(scoreGained);
                ghost.GotEatenByPacman(scoreGained.ToString());

                _gameModeManager.UpgradeFactorToEatGhostsSequentially();

            } else {
                _gameModeManager.currentGameMode = GameMode.DEAD;
                StartCoroutine(PacmanDieAnimation());
            }
        }
    }

    /*
     *  In case of game over, this method will reset the static variables and restart the scene
     */
    public void RestartGame() {
        Level = 0;
        Life = 3;
        Score = 0;
        _factorToGainExtraLife = 1;
        _sceneChanger.LoadLevel();
    }

    public GameMode GetCurrentGameMode() => _gameModeManager.currentGameMode;

    public void RegisterFruit(Fruit fruit) => fruit.SubscribeOnGetCaught(() => AddScore(fruit.points));

    public void RegisterDot(Dot dot) {
        dot.SubscribeOnCaught(OnDotGetCaught);
        _dots.Add(dot);
    }

    public void RegisterGhosts(Ghost ghost) => _ghosts.Add(ghost);

    public void RegisterPlayer(Pacman pacman) {
        _pacman = pacman;
        _pacman.SubscribeOnGetCaughtByGhosts(OnPacmanGetCaughtByGhosts);
    }

    public void SubscribeForGameModeChanges(Action<GameMode> action) =>
        _gameModeManager.SubscribeForGameModeChanges(action);

}
