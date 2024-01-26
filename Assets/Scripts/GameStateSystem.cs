using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    INTRO,
    WAITING_FOR_PLAYERS,
    COUNTDOWN,
    FIGHT,
    GAME_OVER,
    OUTRO
}

public class GameStateSystem : MonoSingleton<GameStateSystem>
{
    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    public float StateTime { get; private set; }

    public void ChangeGameState(GameState state)
    {
        StateTime = 0f;
        CurrentState = state;
        OnGameStateChanged?.Invoke(state);
    }

    private void Update()
    {
        StateTime += Time.deltaTime;

        switch (CurrentState)
        {
            case GameState.INTRO:
            {
                Update_Intro();
                break;
            }
            case GameState.WAITING_FOR_PLAYERS:
            {
                Update_WaitingPlayers();
                break;
            }
            case GameState.COUNTDOWN:
            {
                Update_Countdown();
                break;
            }
            case GameState.FIGHT:
            {
                Update_Fight();
                break;
            }
            case GameState.GAME_OVER:
            {
                Update_GameOver();
                break;
            }
            case GameState.OUTRO:
            {
                Update_Outro();
                break;
            }
        }
    }

    private void Update_Outro()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.INTRO);
        }
    }

    private void Update_Intro()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.WAITING_FOR_PLAYERS);
        }
    }

    private void Update_GameOver()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.INTRO);
        }
    }

    private void Update_Countdown()
    {
        if (StateTime > GameSettings.Instance.Countdown)
        {
            ChangeGameState(GameState.FIGHT);
        }
    }

    private void Update_WaitingPlayers()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.COUNTDOWN);
        }
    }

    private void Update_Fight()
    {
        if (StateTime > GameSettings.Instance.GameTime)
        {
            ChangeGameState(GameState.GAME_OVER);
        }
    }
}
