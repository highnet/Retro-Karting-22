using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using static RaceController;

public class CameraManager : MonoBehaviour
{
    public float currentFieldOfView; // the camera's current field of view
    public float initialFieldOfView; // the camera's initial field of view
    public CinemachineVirtualCamera mainCVCam; // the main cinemachine virtual cam
    public CinemachineVirtualCamera flyOverCVCam; // the flyover cinemachine virtual cam
    public CinemachineVirtualCamera endGameCVCam; // the endgame cinemachine virtual cam
    public bool flyOverSequenceStarted; // the status of wether has the flyover sequence started?
    public bool flyOverSequenceEnded; // the status has the flyover sequence ended?
    public List<CinemachineSmoothPath> dollySmoothPaths; // the list of cinemachine dolly paths for the flyover camera sequence
    public List<Transform> dollyLookAts; // the positions of the cinemachine dolly paths lookat objects for the flyover camera sequence
    public int currentDollyPathIndex; // the index of the current dolly path which the flyover cinemachine virtual camera is currently going through during the flyover sequence
    public RaceController raceController; // the game's race controller
    public RacingUIController racingUIController; // the game's racing UI controller
    public CinemachineOrbitalTransposer endGameCVCamOrbitalTransposer; // the endgame cinemachine virtual camera transposer
    public float endGameCVCamOrbitalValue; // the endgame cinemachine virtal camera transposer orbital value
    public Sequence fadeSequence;

    private void Start()
    {
        mainCVCam.m_Lens.FieldOfView = initialFieldOfView; // set the initial main cinemachine virtual camera field of view
        currentFieldOfView = initialFieldOfView; // set the current field of view to the intifial field of view
        racingUIController = FindObjectOfType<RacingUIController>(); // store a local reference to the game's racing UI controller
    }
    // Update is called once per frame
    void Update()
    {
        mainCVCam.m_Lens.FieldOfView = currentFieldOfView; // set the main cinemachine vritual camera field of view to the current field of view
        if (flyOverSequenceStarted && !flyOverSequenceEnded && (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))) // if  the flyover sequence has started and not ended, and the player inputs either of mouse0, mouse1, space or return key
        {
            TerminateFlyOverSequence(); // terminate and skip the flyover sequence
        }
    }

    private void FixedUpdate()
    {
        if (endGameCVCamOrbitalTransposer) // if the endgame cinemachine virtual camera transposer exists
        {
            endGameCVCamOrbitalTransposer.m_XAxis.Value = endGameCVCamOrbitalValue; // set the endgame cinemachine virtual camera transposer x axis value to the transposer orbital value
        }
    }

    public void ActivateEndGameCam() // activates the endgame camera sequence

    {
        mainCVCam.gameObject.SetActive(false); // deactivate the cinemachine main virtual camera
        endGameCVCam.gameObject.SetActive(true); // activate the cinemachine endgame virtual camera
        endGameCVCamOrbitalTransposer = endGameCVCam.GetCinemachineComponent<CinemachineOrbitalTransposer>(); // store a local reference of the endgame cinemachine virtual camera transposer
        DOTween.To(() => endGameCVCamOrbitalValue, (newValue) => endGameCVCamOrbitalValue = newValue, 180f, 18f); // tween the endgame cinemachine virtal camera transposer orbital value from 0 to 180 in 18 seconds
    }

    public void DoSpeedBoostMovement(float zoomOutDuration, float zoomInDuration,float deltaFieldOfView) // simulates a camera doing a speed boost movement, as it follows behind the kart

    {
        Sequence sequence = DOTween.Sequence(); // create a new sequence
        sequence.Append(DOTween.To(() => currentFieldOfView, (newValue) => currentFieldOfView = newValue, initialFieldOfView + deltaFieldOfView, zoomOutDuration)); // tween the currentFieldOfView from currentFieldOfView to initialfieldOfView + deltaFieldOfView in zoomOutDuration seconds
        sequence.Append(DOTween.To(() => currentFieldOfView, (newValue) => currentFieldOfView = newValue, initialFieldOfView, zoomInDuration)); // then the currentFieldOfView from currentFieldOfView back to the initialFieldOfView in zoomInDuration seconds, back to its original value
    }

    public void DoFlyOverSequence() // executes through the fly over sequence, beginning at the first dolly path

    {
        if (!flyOverSequenceStarted) // check that the fly over sequence hasn't started yet
        {
            flyOverSequenceStarted = true; // flag the flyover sequence as started
            racingUIController.pressToContinueText.gameObject.SetActive(true); // activate the racing UI's press to continue text
            racingUIController.TweenPressToContinueText(); // tween the racing UI's press to continue text
            CinemachineTrackedDolly dolly = flyOverCVCam.GetCinemachineComponent<CinemachineTrackedDolly>(); // get a reference to the fly over cinemachine virtual camera's dolly
            dolly.m_Path = dollySmoothPaths[0]; // set the dolly's path to the first element of the list of cinemachine dolly paths for the flyover camera sequence
            flyOverCVCam.LookAt = dollyLookAts[0]; // set the cinemachine fly over virtual camera's lookat to the first element of the positions of the cinemachine dolly paths lookat objects for the flyover camera sequence
            DOTween.To(() => dolly.m_PathPosition, (newValue) => dolly.m_PathPosition = newValue, 1.0f, 5.0f) // tween the dolly's path position from 0 to 1 in 5 seconds ...
                .OnComplete(() => { // ... and when its finished ...
                    FadeCamInOutBlack(); // ...fade out the camera and then back in in a new dolly path position
                });
        }

    }
    public void NextFlyoverStep() // executes the next part of the flyover sequence, starting at the next index dolly path

    {
        currentDollyPathIndex++; // increment the index of the current dolly path which the flyover cinemachine virtual camera is currently going through during the flyover sequence
        if (flyOverSequenceEnded || currentDollyPathIndex > dollySmoothPaths.Count - 1) // if the flyover sequence has been marked as ended, or  the current dolly path index is greater than the count of dolly paths
        {
            TerminateFlyOverSequence(); // terminate the flyover sequence
            return; // break the flyover sequence
        }
        CinemachineTrackedDolly dolly = flyOverCVCam.GetCinemachineComponent<CinemachineTrackedDolly>();  // get a reference to the fly over cinemachine virtual camera's dolly
        dolly.m_PathPosition = 0f; // teleport the dolly's position back to 0
        dolly.m_Path = dollySmoothPaths[currentDollyPathIndex]; // set the dolly's path to the next dolly path
        flyOverCVCam.LookAt = dollyLookAts[currentDollyPathIndex]; // set the cinemachine fly over virtual camera look at to the next dolly path
        DOTween.To(() => dolly.m_PathPosition, (newValue) => dolly.m_PathPosition = newValue, 1.0f, 5.0f) // tween the dolly's path position from 0 to 1 in 5 seconds ...
            .OnComplete(() => { // ... and when its finished ...
                FadeCamInOutBlack(); // ...fade out the camera and then back in in a new dolly path position
            });
    }

    public void TerminateFlyOverSequence() // terminates the flyover sequence
    {
        flyOverSequenceEnded = true; // flag the flyover sequence as ended
        flyOverCVCam.gameObject.SetActive(false); // deactivate the cinemachine flyover virtual camera
        mainCVCam.gameObject.SetActive(true); // actiave the cinemachine main virtual camera
        racingUIController.pressToContinueText.gameObject.SetActive(false); // deactivate the racing ui's press to continue text
        raceController.racePhase = RacePhase.TimeTrialCountdownRace; // set the race controller's race phase to time trial countdown race
    }

    public void FadeCamInOutBlack() // fades the camera in and out black as it changes view in the flyover sequence
    {
        CinemachineStoryboard storyboard = flyOverCVCam.GetComponent<CinemachineStoryboard>(); // get a reference to the cinemachine fly over virtual camera's storyboard
        fadeSequence = DOTween.Sequence(); // create a new sequence
        fadeSequence.Append(DOTween.To(() => storyboard.m_Alpha, (newValue) => storyboard.m_Alpha = newValue, 1.0f, .25f).SetEase(Ease.Linear) // tween the storyboard's alpha from 0 to 1 in .25 seconds ...
            .OnComplete(() => { // ... and when its finished ...
                NextFlyoverStep(); // ... begin the next flyover step in the flyover sequence
            }));
        fadeSequence.AppendInterval(0.5f); // wait 0.5 seconds
        fadeSequence.Append(DOTween.To(() => storyboard.m_Alpha, (newValue) => storyboard.m_Alpha = newValue, 0.0f, .25f).SetEase(Ease.Linear)); // tween the storyboard's alpha from 1 back to 0 in .25 seconds
    }
}
