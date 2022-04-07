using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState {Intro, MainMenu, Racing}; // the types of game states
    public enum GameMode {NONE, TimeTrial}; // the type of game modes
    public GameState currentGameState; // the current game state
    public GameMode currentGameMode; // the current game mode

    private void Start()
    {
        DontDestroyOnLoad(this); // flag this script not to be destroyed on scene changes
    }

    public void ChangeGameState(GameState gameState,GameMode gameMode)
    {
        currentGameState = gameState; // change the current game state
        currentGameMode = gameMode; // change the current game mode
    }
}
