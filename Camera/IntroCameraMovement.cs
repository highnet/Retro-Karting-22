using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using static GameStateManager;

public class IntroCameraMovement : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera; // the cinemachine virtual camera
    private CinemachineTrackedDolly dolly; // the dolly
    private CinemachineComposer composer; // the composer
    private CinemachineStoryboard storyboard; // the storyboard
    private bool started; // the status of wether the intro camera movement has started or not
    public float forceStartedTimer; // the timer to force start the intro camera movement, it starts at a positive number and counts down to zero, once its zero, the intro camera movement is forced
    public GameObject backgroundCar; // the background car
    public IntroUIController introUIController; // the game's intro ui controller
    private SceneManager sceneManager; // the game's scene manager
    private Tween pathPosition; // the tween of the path's position
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManager>(); // store a local reference to the game's scene manager
        dolly = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>(); // store a local reference of the cinemachine virtual camera's dolly
        composer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>(); // store a local reference of the cinemachine virtual camera's composer
        storyboard = cinemachineVirtualCamera.GetComponent<CinemachineStoryboard>(); // store a local reference of the cinemachine virtual camera's storyboard
        composer.m_TrackedObjectOffset.x = -80.0f; // set the composer's tracked object offset x axis to -80
        storyboard.m_Alpha = 1.0f; // set the storyboard's alpha to 1
        storyboard.m_ShowImage = true; // show the storyboard's image
        pathPosition = (DOTween.To(() => dolly.m_PathPosition, (newValue) => dolly.m_PathPosition = newValue, 2.0f, 200.0f)).SetEase(Ease.InOutSine); // tween the dolly's path position from 0 to 2 in 200 seconds
        DOTween.To(() => storyboard.m_Alpha, (newValue) => storyboard.m_Alpha = newValue, 0.0f, 5.0f).SetEase(Ease.InQuad); // tween the storyboards alpha from 1 to 0 in 5 seconds
    }

    private void Update()
    {
        if (forceStartedTimer > 0) // if the forced start timer is greater than 0
        { 
            forceStartedTimer -= Time.deltaTime; // decrement the timer
            if (forceStartedTimer < 0) // if it has gone under zero
            {
                forceStartedTimer = 0; // set it to zero
            }
        }
        if (!started && (introUIController.numberOfClicks > 0 || forceStartedTimer == 0)) // if the intro camera movement sequence hasnt started, and the user has clicked the screen more than once, or the force start timer has reached 0
        {
            started = true; // flag the intro camera movement sequence as started
            pathPosition.timeScale = 10f; // make the path position tween's timescale go 10 times faster
            DOTween.To(() => composer.m_TrackedObjectOffset.x, (newValue) => composer.m_TrackedObjectOffset.x = newValue, 0.0f, 5.0f); // tween the composer's tracked object offset x axis from -80 to 0 in 5 seconds
        }
    }

    private void FixedUpdate()
    {
        backgroundCar.transform.Translate(Vector3.forward * 0.5f); // move the background car forward
        if (dolly.m_PathPosition == 2.0f) // if the dolly path has reached its final position
        {
            sceneManager.ChangeScene("Main Menu",GameState.MainMenu,GameMode.NONE); // transition the scene to the main menu with an unspecified game mode
        }
    }
}