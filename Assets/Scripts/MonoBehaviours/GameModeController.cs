using System.Collections.Generic;
using UnityEngine;

public class GameModeController : MonoBehaviour {
    private interface IGameModeListener {
        void OnGameModeChanged(GameMode gameMode);
    }

    private class GameModeSettings {
        public GameMode ModeType;
        public int Seconds;
    }

    public enum GameMode { WAITING, INIT, SCATTER, CHASE, FRIGHTENED }

    private Queue<GameModeSettings> _queueGameModes;
    private GameMode _currentGameMode = GameMode.CHASE;
    private bool _isFightened = false;

    public static GameModeController Instance { private set; get; }

    public GameMode GetCurrentGameMode() {
        return _currentGameMode;
    }

    private void Awake() {
        Instance = this;
    }

    void Start() {
        _queueGameModes = new Queue<GameModeSettings>();
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 7 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 7 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 5 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 20 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.SCATTER, Seconds = 5 });
        _queueGameModes.Enqueue(new GameModeSettings { ModeType = GameMode.CHASE, Seconds = 0 }); // this one is permanent
    }

    void Update() {

    }
}
