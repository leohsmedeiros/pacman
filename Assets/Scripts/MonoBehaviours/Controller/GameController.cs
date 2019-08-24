using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to control all the elements of
 *  the game and its states and apply the rules relative to each event.
 *
 *  Will use the 'Managers' to share this responsibility and help it to
 *  control these elements.
 */

[RequireComponent(typeof(FruitManager))]
[RequireComponent(typeof(UiManager))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(GameModeManager))]
[RequireComponent(typeof(SceneChanger))]
public class GameController : MonoBehaviour {

    public static GameController Instance { private set; get; }

    public static int Stage { private set; get; } = 0;
    public static int Score { private set; get; } = 0;
    public static int Life { private set; get; } = 3;

    /*
     * The player will receive one extra life bonus after obtaining 10,000 points,
     * and this factor is used to calculate the next goal to obtaining a new life.
     */
    private static int _factorToGainExtraLife = 1;

    public Settings settings;

    public Node pacmanStarterNode;

    private Node _ghostsHouse;

    private List<Dot> _dots;
    private List<Ghost> _ghosts;

    /*
     * it is used to control when the fruits must be shown up (70 dots on the
     * first time and then on 170 dots eaten)
     */
    private int _eatenDotsAmount = 0;

    /*
     * As long as ghosts were being eaten sequentially the value of score for
     * that will become higher (2 times more valuable). And this factor is used
     * for that. When the game returns from frightened mode, than this factor
     * restarts to 1
     */
    private int _factorToEatGhostsSequentially = 1;

    private Pacman _pacman;

    private StageSettings _stageSettings;

    private SceneChanger _sceneChanger;

    private FruitManager _fruitManager;
    private UiManager _uiManager;
    private SoundManager _soundManager;
    private GameModeManager _gameModeManager;


    IEnumerator PacmanDeathAnimation() {
        _soundManager.PauseBackgroundSource();

        yield return new WaitForSeconds(settings.TimeToGhostsVanishOnPacmanDeath);

        _ghosts.ForEach(ghost => ghost.gameObject.SetActive(false));

        yield return new WaitForSeconds(settings.TimeToStartAnimationOfPacmanDeath);

        _soundManager.PlayPacManDeathSound();
        _pacman.GetComponent<PacmanAnimator>().SetAnimation(PacmanAnimator.PacmanAnimation.DIE);

        yield return new WaitForSeconds(settings.TimeToRestartSceneAfterPacmanDeath);

        LoseLife();
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

        /* Will select the stage settings relative to current Stage */
        if (Stage < settings.SequenceOfStageSettings.Length) {
            _stageSettings = settings.SequenceOfStageSettings[Stage];
        } else {
            int lastIndex = settings.SequenceOfStageSettings.Length - 1;
            _stageSettings = settings.SequenceOfStageSettings[lastIndex];
        }

        _gameModeManager.settings = settings;
        _gameModeManager.stageSettings = _stageSettings;  

        /* Will get the fruit to update on GUI */
        Fruit fruit = _fruitManager.GetFruitByType(_stageSettings.fruitType);

        _uiManager.FruitOnGUI(fruit.GetSprite());
        _uiManager.LifesOnGUI(Life);
        _uiManager.ScoreOnGUI(Score);
        _uiManager.HighScoreOnGUI(PlayerPrefs.GetInt(settings.HighScoreKeyPlayerPrefs, 0));
    }

    private void Start() {
        /* adapt the background sound when GameMode was updated */
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
                    _factorToEatGhostsSequentially = 1;
                    _soundManager.PlaySirenSound();
                    break;
            }
        });

        _gameModeManager.currentGameMode = GameMode.INTRO;
    }


    private void NextStage() {
        Stage++;

        /* if it fits the intermission factor, will redirect to intermission scene */
        if (Stage % settings.IntermissionFactor == 0)
            _sceneChanger.LoadLevel(settings.IntermissionSceneName);
        else
            _sceneChanger.LoadLevel();
    }

    /*
     *  Pacman will lose one life.
     *  If there are more lives to continue, then will update the GUI and
     *  reset all the elements, but the eaten dots and score.
     *  If there are no more lives, then will check if it is a highscore and
     *  then will show the GameOver object and the button to Restart the game.
     */
    private void LoseLife() {
        Life -= 1;
        _uiManager.LifesOnGUI(Life);

        if (Life > 0) {
            _ghosts.ForEach(ghost => {
                ghost.transform.position = _ghostsHouse.GetPosition2D();
                ghost.gameObject.SetActive(true);
                ghost.Reset();
            });

            _pacman.transform.position = pacmanStarterNode.GetPosition2D();
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

    /*
     *  Will add an amount of score. If the current score is higher than the
     *  _factorToGainExtraLife * PointsToGainExtraLife, then pacman will obtain
     *  a new life.
     */
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

    /*
     *  Dots can be energizer or not. If it is an energizer, then on get caught
     *  the ghosts will enter on frightened mode. If it's not, then will check
     *  if the fruit must be shown up or destoyed.
     *  If there are no more dots on stage, go to the next one.
     */
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
            NextStage();
    }

    /*
     *  If the ghost is on frightened mode, the the ghost will be eaten and
     *  pacman will gain a relative score of that.
     *
     *  If not, then will run the pacman death animation, and pacman will
     *  loses a life
     */
    private void OnPacmanGetCaughtByGhosts(Ghost ghost) {
        if (!ghost.isDead && !_gameModeManager.currentGameMode.Equals(GameMode.DEAD)) {
            if (ghost.isFrightened) {
                int scoreGained =
                    settings.GhostFrightenedBaseScore * _factorToEatGhostsSequentially;

                AddScore(scoreGained);
                ghost.GotEatenByPacman(scoreGained.ToString());

                _factorToEatGhostsSequentially *= settings.GhostScoreFactor;

            } else {
                _gameModeManager.currentGameMode = GameMode.DEAD;
                StartCoroutine(PacmanDeathAnimation());
            }
        }
    }


    /*  In case of game over, this method will reset the static variables and restart the scene */
    public void RestartGame() {
        Stage = 0;
        Life = 3;
        Score = 0;
        _factorToGainExtraLife = 1;
        _sceneChanger.LoadLevel();
    }


    public GameMode GetCurrentGameMode() => _gameModeManager.currentGameMode;

    public void SubscribeForGameModeChanges(Action<GameMode> action) =>
        _gameModeManager.SubscribeForGameModeChanges(action);


    /*  At the beginning of the game start all these elements must be registered to GameController */

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

    public void RegisterGhostHouse(GhostHouse ghostHouse) {
        ghostHouse.SubscribeOnGhostIsInsideGhostHouse(ghost => ghost.Revive());
        _ghostsHouse = ghostHouse;
    }

}
