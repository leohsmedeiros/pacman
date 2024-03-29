﻿using System;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The responsibility of this script is to control the current GameMode and
 *  notify all the observers when it changes.
 */

public class GameModeManager : MonoBehaviour {
    private GameMode _currentGameMode = GameMode.INTRO;
    public GameMode currentGameMode {
        set {
            _currentGameMode = value;
            _actionsForGameModeChange.ForEach(action => action.Invoke(_currentGameMode));
        }
        get => _currentGameMode;
    }

    public GameObject readyObject;
    public GameObject gameOverPrefab;

    private List<Action<GameMode>> _actionsForGameModeChange;
    private float _timerIntro = 0;
    private float _timerFrightened = 0;
    private float _timerGameModes = 0;
    private int _lastIndexGameModeSettings = 0;
    private GameObject _gameoverOnScreen;

    public StageSettings stageSettings { set; private get; }
    public Settings settings { set; private get; }


    private void Awake() => _actionsForGameModeChange = new List<Action<GameMode>>();

    private void Update() {
        if (currentGameMode.Equals(GameMode.DEAD))
            return;
        

        if (currentGameMode.Equals(GameMode.INTRO)) {
            IntroModeUpdate();
        } else if (currentGameMode.Equals(GameMode.FRIGHTENED)) {
            FrightenedModeUpdate();
        } else if (currentGameMode.Equals(GameMode.FRIGHTENED_FLASHING)) {
            FrightenedFlashingModeUpdate();
        } else {
            GameModeUpdate();
        }
    }

    private void IntroModeUpdate() {
        _timerIntro += Time.deltaTime;

        float maxTime = settings.TimeToHideReadyObject;

        if (_timerIntro > maxTime) {
            _timerIntro = 0;
            currentGameMode = settings.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
            readyObject.SetActive(false);
        } else {
            readyObject.SetActive(true);
        }
    }

    private void FrightenedModeUpdate() {
        _timerFrightened += Time.deltaTime;

        float maxTime = stageSettings.ghostFrightenedTime;

        if (_timerFrightened > maxTime) {
            _timerFrightened = 0;
            currentGameMode = GameMode.FRIGHTENED_FLASHING;
        }
    }

    private void FrightenedFlashingModeUpdate() {
        _timerFrightened += Time.deltaTime;

        float maxTime = stageSettings.ghostFlashingTime;

        if (_timerFrightened > maxTime) {
            _timerFrightened = 0;
            currentGameMode = settings.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
        }
    }

    /* Will use a timer and the settings to update from scatter to chase mode and vice-versa */
    private void GameModeUpdate() {
        _timerGameModes += Time.deltaTime;

        float maxTime = (_lastIndexGameModeSettings < settings.SequenceOfGameModeSettings.Length - 1) ?
            settings.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Seconds : float.MaxValue;

        if (_timerGameModes > maxTime) {
            _lastIndexGameModeSettings = (_lastIndexGameModeSettings + 1 < settings.SequenceOfGameModeSettings.Length) ?
                _lastIndexGameModeSettings + 1 : settings.SequenceOfGameModeSettings.Length - 1;

            _timerGameModes = 0;
            currentGameMode = settings.SequenceOfGameModeSettings[_lastIndexGameModeSettings].Mode;
        }
    }

    public void SubscribeForGameModeChanges(Action<GameMode> action) => _actionsForGameModeChange.Add(action);

    public void GameOver() => Instantiate(gameOverPrefab);


    public void Reset() {
        _timerIntro = 0;
        _timerFrightened = 0;
        _timerGameModes = 0;
        currentGameMode = GameMode.INTRO;
    }

}
