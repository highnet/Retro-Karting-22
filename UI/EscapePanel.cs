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

    public GameObject centerAnchor;
    public GameObject offscreenAnchor;
    public bool paused;
    public RaceController raceController;
    public SceneManager sceneManager;


    private void Start()
    {
        raceController = FindObjectOfType<RaceController>();
        sceneManager = FindObjectOfType<SceneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Unpause()
    {
        transform.DOMove(offscreenAnchor.transform.position, .33f).SetEase(Ease.InOutQuad).SetUpdate(true);
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1.0f;
        paused = false;
        if (PlayerPrefs.GetFloat("SoundMuted?") == 0)
        {
            AudioListener.volume = 1f;
        }
    }

    public void Pause()
    {

        if (raceController.lapNumber != raceController.totalNumberOfLaps)
        {
            transform.DOMove(centerAnchor.transform.position, .33f).SetEase(Ease.InOutQuad).SetUpdate(true);
            Time.timeScale = 0.0f;
            paused = true;
            AudioListener.volume = 0f;
        }
    }

    public void QuitRace()
    {
        Unpause();
        sceneManager.ChangeScene("Main Menu", GameState.MainMenu, GameMode.NONE);
    }

    public void RestartRace()
    {
        Unpause();
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        sceneManager.ChangeScene(scene.name, GameState.Racing, raceController.gameStateManager.currentGameMode);
    }
}
