using UnityEngine;

[CreateAssetMenu(fileName = "New Game Settings", menuName = "Settings")]
public class Settings : ScriptableObject {

    public string PlayerTag = "Player";
    public string GhostTag = "Ghost";
    public string NodeTag = "Node";
    public string HighScoreKeyPlayerPrefs = "HighScore";
    public string IntermissionSceneName = "Intermission";

    public int DotScore = 10;
    public int EnergizerScore = 50;
    public int GhostFrightenedBaseScore = 200;
    public int GhostScoreFactor = 2;
    public int PointsToGainExtraLife = 10000;
    public int DotsAmountToShowFruitFirstTime = 70;
    public int DotsAmountToShowFruitSecondTime = 170;
    public int IntermissionFactor = 3;

    public float TimeToHideReadyObject = 5f;
    public float TimeToGhostsVanishOnPacmanDeath = 1f;
    public float TimeToStartAnimationOfPacmanDeath = 0.5f;
    public float TimeToRestartSceneAfterPacmanDeath = 1.5f;
    public float TimeShowingScorePointsGained = 2;

    [SerializeField]
    public GameModeSettings[] SequenceOfGameModeSettings = {
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 0 } // this one is permanent
    };

    [SerializeField]
    public StageSettings[] SequenceOfStageSettings = {
        new StageSettings { fruitType = FruitType.CHERRY, ghostFrightenedTime = 6, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.STRAWBERRY, ghostFrightenedTime = 5, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.ORANGE, ghostFrightenedTime = 4, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.ORANGE, ghostFrightenedTime = 3, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.APPLE, ghostFrightenedTime = 2, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.APPLE, ghostFrightenedTime = 5, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.MELON, ghostFrightenedTime = 2, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.MELON, ghostFrightenedTime = 2, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.GALAXIAN, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.GALAXIAN, ghostFrightenedTime = 5, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.BELL, ghostFrightenedTime = 2, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.BELL, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 3, ghostFlashingTime = 5 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 0, ghostFlashingTime = 0 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 1, ghostFlashingTime = 3 },
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 0, ghostFlashingTime = 0 } // this one is permanent
    };


}
