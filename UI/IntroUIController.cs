using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Audio;
using static GameStateManager;

public class IntroUIController : MonoBehaviour
{
    public float numberOfClicks; // the number of times the user has clicked in the intro
    private SceneManager sceneManager; // the scene manager
    public TextMeshProUGUI pressToContinueText; // the press to continue text
    private float pressToContinueTextAlpha = 255f; // the press to continue text RBGA alpha 
    public Image titleImage; // the title image
    private float titleImageAlpha = 0f; // the title image RBGA alpha
    public Mixer mixerMain; // the mixer main
    private float pressToContinueTextStartScaleX; // the press to continue text start scale x magnitude
    private float pressToContinueTextStartScaleY; // the press to continue text start scale y magnitude
    public bool initialized; // the status of wether LateStart() has initialized

    private void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManager>(); // store a local reference to the scene's manager
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>(); // store a local reference to the main mixer
        if(SaveSystem.LoadRecords() == null || PlayerPrefs.GetInt("ResettedRecordsSince280420221221", 0) == 0) // check if there are no records on file or the records havent been force resetted since a specific timestamp with the force reset tag
        {
            SaveSystem.SaveRecords(SaveSystem.GenerateDefaultRecords()); // generate default records and save them on file
            PlayerPrefs.SetInt("ResettedRecordsSince280420221221", 1); // set the force reset tag on player prefs
            PlayerPrefs.SetInt("ChosenCharacterIndex", 0); // set the default chosen character index
            PlayerPrefs.SetInt("ChosenKartBodyIndex", 0); // set the default chosen kart index
            PlayerPrefs.Save(); // save the player prefs
        }

        if (SaveSystem.LoadCollectibles() == null || PlayerPrefs.GetInt("ResettedCollectiblesSince280420221221", 0) == 0)
        {
            SaveSystem.SaveCollectibles(SaveSystem.GenerateDefaultCollectibles());
            PlayerPrefs.SetInt("ResettedCollectiblesSince280420221221", 1);
                PlayerPrefs.Save();
        }

        if (SaveSystem.LoadGhostRider()  == null || PlayerPrefs.GetInt("ResettedGhostRiderSince280420221221", 0) == 0)
        {
            SaveSystem.SaveGhostRider(SaveSystem.GenerateDefaultGhostRider());
            PlayerPrefs.SetInt("ResettedGhostRiderSince280420221221", 1);
            PlayerPrefs.Save();
        }

        pressToContinueTextStartScaleX = pressToContinueText.transform.localScale.x; // set the press to continue start scale x magnitude
        pressToContinueTextStartScaleY = pressToContinueText.transform.localScale.y; // set the press to continue start scale y magnitude
        DOTween.To(() => pressToContinueTextAlpha, (newValue) => pressToContinueTextAlpha = newValue, 0f, 600f).SetEase(Ease.InOutFlash, 300f, 0f); // tween the press to continue text alpha as an oscillation
        pressToContinueText.rectTransform.DOScale(new Vector3(pressToContinueTextStartScaleX + 0.3f, pressToContinueTextStartScaleY + 0.3f, 1f), 1.0f).SetLoops(-1, LoopType.Yoyo); // tween the press to continue scale as an oscillation
        DOTween.To(() => titleImageAlpha, (newValue) => titleImageAlpha = newValue, 255f, 3f).SetEase(Ease.InQuad); // tween the title image alpha from 0 to 255 in 3 seconds
        StartCoroutine(LateStart()); // start the late start co routine
    }

    public IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f); // wait 0.5 seconds
        UpdateMutedStatus(); // update the muted status
        initialized = true; // flag the script as initialized
    }

    private void Update()
    {
        pressToContinueText.color = new Color32(120, 68, 231, (byte) pressToContinueTextAlpha); // update the press to continue text RBGA color alpha
        titleImage.color = new Color32(255, 255, 255, (byte) titleImageAlpha); // update the title image RBGA color alpha
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape)) // check for player input
        {
            numberOfClicks++; // increment the number of clicks
            ChangeScene(); // try to change the scene
        }
    }
    public void UpdateMutedStatus()
    {
        mixerMain.UpdateMutedStatus(); // update the muted status
    }

    public void ChangeScene()
    {
        if (initialized && numberOfClicks > 1) // check if initialized and number of clicks is greater than 1
        {
            sceneManager.ChangeScene("Main Menu",GameState.MainMenu,GameMode.NONE); // change the scene
        }
    }
}
