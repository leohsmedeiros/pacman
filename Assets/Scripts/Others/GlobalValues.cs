public class GlobalValues {
    public static readonly string PlayerTag = "Player";
    public static readonly string GhostTag = "Ghost";
    public static readonly string NodeTag = "Node";
    public static readonly string HighScoreKeyPlayerPrefs = "HighScore";

    public static readonly int DotScore = 10;
    public static readonly int EnergizerScore = 10;
    public static readonly int GhostFrightenedBaseScore = 200;
    public static readonly int GhostScoreFactor = 2;
    public static readonly int PointsToGainExtraLife = 10000;
    public static readonly float TimeToGhostsVanishOnPacmanDeath = 1f;
    public static readonly float TimeToStartAnimationOfPacmanDeath = 0.5f;
    public static readonly float TimeToRestartSceneAfterPacmanDeath = 1.5f;

    public static readonly int[] _timeForFrightenedModeByLevel = { 6, 5, 4, 3, 2, 5, 2, 2, 1, 5, 2, 1, 1, 3, 1, 1, 0, 1, 0 };

    public static readonly GameModeSettings[] _gameGameModesSettingsArray = {
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 7 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 20 },
        new GameModeSettings { Mode = GameMode.SCATTER, Seconds = 5 },
        new GameModeSettings { Mode = GameMode.CHASE, Seconds = 0 } // this one is permanent
    };

}
