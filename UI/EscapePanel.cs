using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static RaceController;
using static GameStateManager;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EscapePanel : MonoBehaviour
{

    public GameObject centerAnchor; // the escape panel center anchor
    public GameObject offscreenAnchor; // the escape panel offscreen anchor
    public bool paused; // the game paused status
    public RaceController raceController; // the race controller
    public SceneManager sceneManager; // the scene manager
    public UserSettings userSettings; // the user settings

    private void Start()
    {
        raceController = FindObjectOfType<RaceController>(); // store a local reference to the race controller
        sceneManager = FindObjectOfType<SceneManager>(); // store a local reference to the scene manager
        userSettings = FindObjectOfType<UserSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // check for player pause input
        {
            if (paused) // check if we are paused
            {
                Unpause(); // unpause the game
            }
            else // otherwise
            {
                Pause(); // pause the game
            }
        }
    }

    public void Unpause()
    {
        transform.DOMove(offscreenAnchor.transform.position, .33f).SetEase(Ease.InOutQuad).SetUpdate(true); // tween the panel to its offscreen position
        EventSystem.current.SetSelectedGameObject(null); // unselect the event system selected object
        Time.timeScale = 1.0f; // set the game times scale to 1
        paused = false; // flag the game as unpaused
        if (!userSettings.soundMuted) // check the user settings for the sound being unmuted
        {
            AudioListener.volume = 1f; // unmute the sound
        }
    }

    public void Pause()
    {

        if (raceController.lapNumber != raceController.totalNumberOfLaps) // check if the user is still racing
        {
            transform.DOMove(centerAnchor.transform.position, .33f).SetEase(Ease.InOutQuad).SetUpdate(true); // tween the panel to its center position
            Time.timeScale = 0.0f; // set the game time scale to 0
            paused = true; // flag the game sa paused
            AudioListener.volume = 0f; // mute the sound
        }
    }

    public void QuitRace()
    {
        Unpause(); // unpause the game
        sceneManager.ChangeScene("Main Menu", GameState.MainMenu, GameMode.NONE); // change the scene
    }

    public void RestartRace()
    {
        Unpause(); // unpause the game
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene(); // get the currently active scene
        sceneManager.ChangeScene(scene.name, GameState.Racing, raceController.gameStateManager.currentGameMode); // change the scene to the current scene
    }
}
