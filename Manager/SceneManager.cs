using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GameStateManager;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameStateManager gameStateManager; // the game's game state manager

    public void ChangeScene(string sceneName, GameState gameState,GameMode gameMode)
	{
        DOTween.KillAll(); // kill all tweens
        DOTween.Clear(); // clear all tweens
        gameStateManager.ChangeGameState(gameState, gameMode); // change the game's state
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // load the new scene
	}

    private void Start()
    {
        DontDestroyOnLoad(this); // flag this script not to be destroy on a scene change
    }

}
