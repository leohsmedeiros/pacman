public static class GlobalValues {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";
    public static readonly string HighScoreKeyPlayerPrefs = "HighScore";

    public static readonly int DotScore = 10;
    public static readonly int EnergizerScore = 50;
    public static readonly int GhostFrightenedBaseScore = 200;
    public static readonly int GhostScoreFactor = 2;
    public static readonly int PointsToGainExtraLife = 10000;
    public static readonly int DotsAmountToShowFruitFirstTime = 70;
    public static readonly int DotsAmountToShowFruitSecondTime = 170;

    public static readonly float TimeToGhostsVanishOnPacmanDeath = 1f;
    public static readonly float TimeToStartAnimationOfPacmanDeath = 0.5f;
    public static readonly float TimeToRestartSceneAfterPacmanDeath = 1.5f;
    public static readonly float TimeShowingScorePointsGained = 2;

    public static readonly GameModeSettings[] SequenceOfGameModeSettings = {
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 0 } // this one is permanent
    };

    public static readonly StageSettings[] SequenceOfStageSettings = {
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
        new StageSettings { fruitType = FruitType.KEY, ghostFrightenedTime = 0, ghostFlashingTime = 0 }
    };

}
