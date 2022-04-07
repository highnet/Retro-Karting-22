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
    public float numberOfClicks;
    private SceneManager sceneManager;
    public TextMeshProUGUI pressToContinueText;
    private float pressToContinueTextAlpha = 255f;
    public Image titleImage;
    private float titleImageAlpha = 0f;
    public Mixer mixerMain;
    private float pressToContinueTextStartScaleX;
    private float pressToContinueTextStartScaleY;
    public bool initialized;

    private void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManager>();
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>();
        if(SaveSystem.LoadRecords() == null || PlayerPrefs.GetInt("ResettedRecordsSince240320221803", 0) == 0)
        {
            SaveSystem.SaveRecords(SaveSystem.GenerateDefaultRecords());
            PlayerPrefs.SetInt("ResettedRecordsSince240320221803", 1);
            PlayerPrefs.SetInt("ChosenCharacterIndex", 0);
            PlayerPrefs.SetInt("ChosenKartBodyIndex", 0);
            PlayerPrefs.Save();
        }
        pressToContinueTextStartScaleX = pressToContinueText.transform.localScale.x;
        pressToContinueTextStartScaleY = pressToContinueText.transform.localScale.y;
        DOTween.To(() => pressToContinueTextAlpha, (newValue) => pressToContinueTextAlpha = newValue, 0f, 600f).SetEase(Ease.InOutFlash, 300f, 0f);
        pressToContinueText.rectTransform.DOScale(new Vector3(pressToContinueTextStartScaleX + 0.3f, pressToContinueTextStartScaleY + 0.3f, 1f), 1.0f).SetLoops(-1, LoopType.Yoyo);
        DOTween.To(() => titleImageAlpha, (newValue) => titleImageAlpha = newValue, 255f, 3f).SetEase(Ease.InQuad);
        StartCoroutine(LateStart());
    }

    public IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateMutedStatus();
        initialized = true;
    }

    private void Update()
    {
        pressToContinueText.color = new Color32(120, 68, 231, (byte) pressToContinueTextAlpha);
        titleImage.color = new Color32(255, 255, 255, (byte) titleImageAlpha);
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape))
        {
            numberOfClicks++;
            ChangeScene();
        }
    }
    public void UpdateMutedStatus()
    {
        mixerMain.UpdateMutedStatus();
    }

    public void ChangeScene()
    {
        if (initialized && numberOfClicks > 1)
        {
            sceneManager.ChangeScene("Main Menu",GameState.MainMenu,GameMode.NONE);
        }
    }
}
