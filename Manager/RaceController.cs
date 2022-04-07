using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static GameStateManager;

public class RaceController : MonoBehaviour
{
    public enum RacePhase {NONE,TimeTrialGameStart,TimeTrialCameraFlyOver,TimeTrialCountdownRace,TimeTrialRace,TimeTrialGameEnd}

    public int lapNumber;
    private PrefabGarage prefabGarage;
    private GameObject spawnPoint;
    public GameObject playerPrefab;

    public int totalNumberOfLaps;
    public List<Checkpoint> requiredCheckpoints;
    public float totalLapTimer;
    public float currentLapTimer;
    public GameStateManager gameStateManager;

    public CameraManager cameraManager;

    public RacePhase racePhase;

    public float raceStartCountdownTimer = 6;
    public KartController kartController;
    public CharacterController characterController;

    public FinishLine finishLine;

    public List<float> lapTimes;

    public UIHintsWindow hintsWindow;

    private void Start()
    {
        prefabGarage = FindObjectOfType<PrefabGarage>();
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn Point");
        gameStateManager = GameObject.FindObjectOfType<GameStateManager>();

        GameObject player = GameObject.Instantiate(playerPrefab,spawnPoint.transform.position, Quaternion.identity);
        GameObject character = GameObject.Instantiate(prefabGarage.characterPrefabs[PlayerPrefs.GetInt("ChosenCharacterIndex")], spawnPoint.transform.position, Quaternion.identity);
        GameObject kart = GameObject.Instantiate(prefabGarage.kartBodyPrefabs[PlayerPrefs.GetInt("ChosenKartBodyIndex")], spawnPoint.transform.position, Quaternion.identity);
        GameObject connectionPoint = GameObject.FindGameObjectWithTag("PlayerCharacterKartConnectionPoint");

        character.transform.parent = connectionPoint.transform;
        kart.transform.parent = connectionPoint.transform;

        kartController = player.GetComponentInChildren<KartController>();
        characterController = player.GetComponentInChildren<CharacterController>();

        kartController.AttachToCharacterAndKart();

        kartController.transform.eulerAngles = spawnPoint.transform.eulerAngles;

        cameraManager.mainCVCam.LookAt = kartController.transform;
        cameraManager.mainCVCam.Follow = kartController.transform;

        cameraManager.endGameCVCam.LookAt = kartController.transform;
        cameraManager.endGameCVCam.Follow = kartController.transform;
        
        finishLine = GetComponentInChildren<FinishLine>();

        hintsWindow = FindObjectOfType<UIHintsWindow>();
    }


    private void Update()
    {
        if (racePhase == RacePhase.NONE && gameStateManager.currentGameMode == GameMode.TimeTrial)
        {
            racePhase = RacePhase.TimeTrialGameStart;
        }

        if (racePhase == RacePhase.TimeTrialGameStart)
        {
            racePhase = RacePhase.TimeTrialCameraFlyOver;
        }

        if (racePhase == RacePhase.TimeTrialCameraFlyOver)
        {
            cameraManager.DoFlyOverSequence();
        }

        if (racePhase == RacePhase.TimeTrialCountdownRace)
        {
            raceStartCountdownTimer -= Time.deltaTime;
            if (raceStartCountdownTimer < 0)
            {
                raceStartCountdownTimer = 0;
            }
        }

        if (racePhase == RacePhase.TimeTrialCountdownRace && raceStartCountdownTimer < 3 && !kartController.engineIsRunning)
        {
            characterController.StartEngine();
            kartController.engineIsRunning = true;
        }

        if (racePhase == RacePhase.TimeTrialCountdownRace && raceStartCountdownTimer == 0)
        {
            if (gameStateManager.currentGameMode == GameMode.TimeTrial)
            {
                racePhase = RacePhase.TimeTrialRace;
            }
            kartController.controllable = true;
            characterController.SayAreYouReady();
        }




        if (racePhase == RacePhase.TimeTrialRace)
        {
            currentLapTimer += Time.deltaTime;
            totalLapTimer += Time.deltaTime;
        }
    }
}
