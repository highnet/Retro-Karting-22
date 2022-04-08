using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static GameStateManager;

public class RaceController : MonoBehaviour
{
    public enum RacePhase {NONE,TimeTrialGameStart,TimeTrialCameraFlyOver,TimeTrialCountdownRace,TimeTrialRace,TimeTrialGameEnd} // the types of race phases
    public int lapNumber; // the lap number
    private PrefabGarage prefabGarage; // the prefab garage
    private GameObject spawnPoint; // the spawn point
    public GameObject playerPrefab; // the player prefab
    public int totalNumberOfLaps; // the toal number of laps
    public List<Checkpoint> requiredCheckpoints; // the list of required checkpoints
    public float totalLapTimer; // the toal lap timer
    public float currentLapTimer; // the current lap timer
    public GameStateManager gameStateManager; // the game state manager
    public CameraManager cameraManager; // the camera manager
    public RacePhase racePhase; // the race phase
    public float raceStartCountdownTimer = 6; // the race start countdown timer
    public KartController kartController; // the kart controller
    public CharacterController characterController; // the characer controller
    public FinishLine finishLine; // the finish line
    public List<float> lapTimes; // the list of lap times
    public UIHintsWindow hintsWindow; // the hints window

    private void Start()
    {
        prefabGarage = FindObjectOfType<PrefabGarage>(); // store a local reference to the prefab garage
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point"); // store a local reference to the spawn point
        gameStateManager = GameObject.FindObjectOfType<GameStateManager>(); // store a local reference to the game state manager
        GameObject player = GameObject.Instantiate(playerPrefab,spawnPoint.transform.position, Quaternion.identity); // instantiate the player prefab
        GameObject character = GameObject.Instantiate(prefabGarage.characterPrefabs[PlayerPrefs.GetInt("ChosenCharacterIndex")], spawnPoint.transform.position, Quaternion.identity); // instantiate the character prefab
        GameObject kart = GameObject.Instantiate(prefabGarage.kartBodyPrefabs[PlayerPrefs.GetInt("ChosenKartBodyIndex")], spawnPoint.transform.position, Quaternion.identity); // instantiate the kart body prefab
        GameObject connectionPoint = GameObject.FindGameObjectWithTag("PlayerCharacterKartConnectionPoint"); // store a local reference to the connection point that will connect the character and the kart
        character.transform.parent = connectionPoint.transform; // connect  the character to the connection point
        kart.transform.parent = connectionPoint.transform; // connect the kart to the connection point
        kartController = player.GetComponentInChildren<KartController>(); // store a local reference to the kart controller
        characterController = player.GetComponentInChildren<CharacterController>(); // store a local reference to the character controller
        kartController.AttachToCharacterAndKart(); // attach the kart and character together
        kartController.transform.eulerAngles = spawnPoint.transform.eulerAngles; // match the rotation of the kart controller to the spawn point
        cameraManager.mainCVCam.LookAt = kartController.transform; // set the cinemachine main virtual camera lookat to the kart controller
        cameraManager.mainCVCam.Follow = kartController.transform; // set the cinemachine main virtual camera follow to the kart controller
        cameraManager.endGameCVCam.LookAt = kartController.transform; // set the cinemachine end game virtual camera lookat to the kart controller
        cameraManager.endGameCVCam.Follow = kartController.transform; // set the cinemachine end game virtual camera folow to the kart c ontroller
        finishLine = GetComponentInChildren<FinishLine>(); // store a local reference to the finsih line
        hintsWindow = FindObjectOfType<UIHintsWindow>(); // store a local reference to the hints window
    }

    private void Update()
    {
        if (racePhase == RacePhase.NONE && gameStateManager.currentGameMode == GameMode.TimeTrial) // if the race phase is none and the game mode is time trial
        {
            racePhase = RacePhase.TimeTrialGameStart; // set the race phase to time trial game start
        }
        if (racePhase == RacePhase.TimeTrialGameStart) // if the race phase is time trial game start
        {
            racePhase = RacePhase.TimeTrialCameraFlyOver; // set the race phase to time trial camera fly over
        }
        if (racePhase == RacePhase.TimeTrialCameraFlyOver) // if the race phase is time trial camera fly over
        {
            cameraManager.DoFlyOverSequence(); // start the camera flyover sequence
        }
        if (racePhase == RacePhase.TimeTrialCountdownRace) // if the race phase is time trial countdown race
        {
            raceStartCountdownTimer -= Time.deltaTime; // subtract from the race start countdown timer
            if (raceStartCountdownTimer < 0) // if the race countdown timer has gone below zero
            {
                raceStartCountdownTimer = 0; // set the race countdown timer to zero
            }
        }
        if (racePhase == RacePhase.TimeTrialCountdownRace && raceStartCountdownTimer < 3 && !kartController.engineIsRunning) // if the race phase is time trial countdown race and the race countdown timer is less than 3 and the kart controller engine is not running
        {
            characterController.StartEngine(); // start the engine
            kartController.engineIsRunning = true; // flag the engine is running as true
        }
        if (racePhase == RacePhase.TimeTrialCountdownRace && raceStartCountdownTimer == 0) // if the race phase is time trial countdown race and the race start countdown timer is equal to zero
        {
            if (gameStateManager.currentGameMode == GameMode.TimeTrial) // if the current game mode is time trial
            {
                racePhase = RacePhase.TimeTrialRace; // set the race phase to time trial race
            }
            kartController.controllable = true; // flag the kart as controllable
            characterController.SayAreYouReady(); // play the character's are you ready sound
        }
        if (racePhase == RacePhase.TimeTrialRace) // if the race phase is time trial race
        {
            currentLapTimer += Time.deltaTime; // increment the current lap timer
            totalLapTimer += Time.deltaTime; // increment the total lap timer
        }
    }
}
