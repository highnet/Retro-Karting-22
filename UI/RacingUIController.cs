using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class RacingUIController : MonoBehaviour
{
    private Rigidbody rigidBody; // the kart controller's rigid body
    private Mixer mixerMain; // the game's main mixer
    public TextMeshProUGUI speedText; // the speedometer text
    public TextMeshProUGUI totalLapTimerText; // the toal lap timer text
    public RaceController raceController; // the game's race controller
    public TextMeshProUGUI pressToContinueText; // the press to continue text
    public float pressToContinueTextAlpha = 255f; // the RGBA alpha value of the press to continue text
    public TextMeshProUGUI goldTimerText; // the gold timer text
    public TextMeshProUGUI silverTimerText; // the silver timer text
    public TextMeshProUGUI bronzeTimerText; // the bronze timer text
    private float goldRecord; // the time in seconds it takes to beat gold record
    private float silverRecord;  // the time in seconds it takes to beat the silver record
    private float bronzeRecord; // the time in seconds it takes to beat the bronze record 
    public Color goldColor; // the color of the gold text/badge
    public Color silverColor; // the color of the silver text/badge
    public Color bronzeColor; // the color of the bronze text/badge
    private bool timerTicking; // the status of wether the race timer is ticking or not
    private float totalLapTimerTextStartScaleX; // the total lap timer text start scale x magnitude
    private float totalLapTimerTextStartScaleY; // the total lap timer text start scale y magnitude
    private float pressToContinueTextStartScaleX; // the press to continue text start scale x magnitude
    private float pressToContinueTextStartScaleY; // the press to continue text start scale y magnitude
    public TextMeshProUGUI coinsText; // the coins text
    public TextMeshProUGUI usableSpeedsupText; // the usable speedups text
    public KartController kartController; // the kart controller
    private float coinTextStartScaleX; // the coins text start scale x magnitude
    private float coinsTextStartScaleY; // the coins text start scale y magnitude
    private float usableSpeedupsTextStartScaleX; // the usable speedsups text start scale x magnitude
    private float usableSpeedupsTextStartScaleY; // the usable speedups text start scale y magnitude
    public Image countdownImg; // the countdown image
    public Sprite countdown3Sprite; // the sprite for the countdown number 3
    public Sprite countdown2Sprite; // the sprite for the countdown number 2
    public Sprite countdown1Sprite; // the sprite for the countdown number 1
    public Sprite countdownGoSprite; // the srpite for the countdown go
    public float countdownImgAlpha; // the RGBA alpha value of the countdown image 
    public bool countdown3Done; // the flag to indicate the countdown number 3 sprite is already done showing
    public bool countdown2Done; // the flag to indicate the countdown number 2 sprite is already done showing
    public bool countdown1Done; // the flag to indicate the countdown number 1 sprite is already done showing
    public bool countdownGoDone; // the flag to indicate the countdown go is sprite already done and showing
    public GameObject centerAnchor; // the center anchor of the racing UI
    public GameObject countdownUpAnchor; // the up anchor for the countdown sequence
    public GameObject countdownDownAnchor; // the down anchor for the countdown sequence
    public AudioClipPlayer finishArchAudioClipPlayer; // the audio clip player of the finish arch
    public GameObject endOfRacePanel; // the end of race panel game object
    public TextMeshProUGUI lapCounterText; // the lap counter text
    public GameObject globalVolume; // the game's global volume game object
    public UserSettings userSettings; // the user's settings
    public UIHintsWindow hintsWindow; // the hints window
    private void Start()
    {
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>(); // store a local reference to the game's main mixer
        rigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>(); // store a local reference to the kart's rigid body
        globalVolume = GameObject.FindGameObjectWithTag("Global Volume"); // store a local reference to the game's global volume
        userSettings = GameObject.FindGameObjectWithTag("User Settings").GetComponent<UserSettings>(); // store a local reference to the user's settings
        kartController = GameObject.FindObjectOfType<KartController>(); // store a local reference to the kart controller
        finishArchAudioClipPlayer = GameObject.FindGameObjectWithTag("Finish").GetComponent<AudioClipPlayer>(); // store a local reference to the finish arch's kart controller
        Records records = SaveSystem.LoadRecords(); // load the records from the save system
        List<RecordEntry> entries; // generate an empty entries list
        records.registry.TryGetValue(raceController.finishLine.track.ToString(), out entries); // get the track's entries list
        UpdateRecordTimerTexts(entries); // update the record timer texts with the track's entries list
        totalLapTimerTextStartScaleX = totalLapTimerText.transform.localScale.x; // store the total lap timer text start scale x magnitude
        totalLapTimerTextStartScaleY = totalLapTimerText.transform.localScale.y; // store the total lap timer text start scale y magnitude
        pressToContinueTextStartScaleX = pressToContinueText.transform.localScale.x; // store the press to continue text start scale x magnitude
        pressToContinueTextStartScaleY = pressToContinueText.transform.localScale.y; // store the press to continue text start scale y magnitude 
        coinTextStartScaleX = coinsText.transform.localScale.x; // store the coins text start scale x magnitude
        coinsTextStartScaleY = coinsText.transform.localScale.y; // store the coins text start scale y magnitude
        usableSpeedupsTextStartScaleX = usableSpeedsupText.transform.localScale.x; // store the usable speedups text start scale x magnitude
        usableSpeedupsTextStartScaleY = usableSpeedsupText.transform.localScale.y; // store the usable speedups text start scale y magnitude
        coinsText.text = kartController.coinCount.ToString(); // update the coints text with the kart controller's coin count
        usableSpeedsupText.text = kartController.usableSpeedups.ToString(); // update the usable speedups text with the kart controller's usable speedups count
        lapCounterText.text = (raceController.lapNumber-1) + "/" + (raceController.totalNumberOfLaps-1); // update the laps counter text with the race controller's lap number counts
        UpdateMutedStatus(); // update the game's muted status
        UpdateRetroFilterStatus(); // update the game's retro filter status
        hintsWindow = FindObjectOfType<UIHintsWindow>(); // store a local reference to the hints window
    }
    public void UpdateRecordTimerTexts(List<RecordEntry> entries)
    {
        goldRecord = entries[0].time; // get the gold record from the first index of the entries array
        TimeSpan timespan1 = TimeSpan.FromSeconds(goldRecord); // convert the gold records seconds to a timespan object
        goldTimerText.text = timespan1.ToString(@"hh\:mm\:ss") + " (" + entries[0].author + ")"; // set the gold timer text with the appropiate string format
        goldTimerText.color = goldColor; // set the gold timer text with the gold color
        silverRecord = entries[1].time; // get the silver record from the first index of the entries array
        TimeSpan timespan2 = TimeSpan.FromSeconds(silverRecord);  // convert the silver records seconds to a timespan object
        silverTimerText.text = timespan2.ToString(@"hh\:mm\:ss") + " (" + entries[1].author + ")"; // set the silver timer text with the appropiate string format
        silverTimerText.color = silverColor; // set the gold timer text with the silver color
        bronzeRecord = entries[2].time; // get the bronze record from the first index of the entries array
        TimeSpan timespan3 = TimeSpan.FromSeconds(bronzeRecord);  // convert the bronze records seconds to a timespan object
        bronzeTimerText.text = timespan3.ToString(@"hh\:mm\:ss") + " (" + entries[2].author + ")"; // set the bronze timer text with the appropiate string format
        bronzeTimerText.color = bronzeColor; // set the gold timer text with the bronze color
    }
    void Update()
    {
        if (rigidBody) // if the rigidbody exists
        {
            float speed = rigidBody.velocity.magnitude; // get the speed from the rigid body's velocity magnitude
            speedText.text = Mathf.Round(speed) + " km/h"; // set the speedometer text with the rounded speed and add the "km/h" unit suffix to it
        }
        float time = raceController.totalLapTimer; // get the time in seconds from the race controller's total lap timer
        TimeSpan timeSpan = TimeSpan.FromSeconds(time); // convert the lap time in seconds to a timespan boject
        totalLapTimerText.text = timeSpan.ToString(@"hh\:mm\:ss"); // convert the lap time in seconds to the appropiate string format
        if (time < goldRecord) // else check if the lap time is less than the gold record
        {
            totalLapTimerText.color = goldColor; // set the total lap timer text color to the gold color
        } else if (time < silverRecord) // else check if the lap time is less than the silver record
        {
            totalLapTimerText.color = silverColor; // set the total lap timer text color to the silver color
        } else if (time < bronzeRecord) // else check if the lap time is less than the bronze record
        {
            totalLapTimerText.color = bronzeColor; // set the total lap timer text color to the bronze color
        }
        else // else
        {
            totalLapTimerText.color = Color.white;  // set the total lap timer text color to the white color
        }
        pressToContinueText.color = new Color32(255, 255, 255, (byte)pressToContinueTextAlpha); // update the press to continue text color alpha
        if (!timerTicking && raceController.totalLapTimer > 0) // check if the timer is not ticking and the total lap timer is greater than 0
        {
            timerTicking = true; // flag the timer ticking as true
            totalLapTimerText.rectTransform.DOScale(new Vector3(totalLapTimerTextStartScaleX + 0.1f, totalLapTimerTextStartScaleY + 0.1f, 1f), .5f).SetLoops(-1, LoopType.Yoyo); // tween the total lap timer text scale to oscillate as it ticks 1 time per second
        }
        if (!countdown3Done && raceController.raceStartCountdownTimer <= 3.0f && raceController.raceStartCountdownTimer > 2f) // check if the countdown 3 is not done and the race start countdown timer is less than or equal to three and greater than two
        {
            countdown3Done = true; // flag the countdown 3 as done
            countdownImgAlpha = 1.0f; // set the countdown image alpha to 1
            countdownImg.sprite = countdown3Sprite; // set the countdown image sprite to the countdown 3 sprite
            finishArchAudioClipPlayer.PlayOneShot(0, 3, 1.0f, true); // play the countdown horn
            TweenCountdownSprite(); // tween the countdown sprite
        }
        else if(!countdown2Done && raceController.raceStartCountdownTimer <= 2.0f && raceController.raceStartCountdownTimer > 1f) // check if the countdown 2 is not done and the race start countdown timer is less than or equal to two and greater than one
        {
            countdown2Done = true; // flag the countdown 2 as done
            countdownImgAlpha = 1.0f; // set the countdown image alpha to 1
            countdownImg.sprite = countdown2Sprite; // set the countdown image sprite to the countdown 2 sprite
            finishArchAudioClipPlayer.PlayOneShot(0, 3, 1.0f, true); // play the countdown horn
            TweenCountdownSprite(); // tween the countdown sprite
        }
        else if (!countdown1Done && raceController.raceStartCountdownTimer <= 1.0f && raceController.raceStartCountdownTimer > 0f) // check if the countdown 1 is not done and the race start countdown timer is less than or equal to one and greater than zero
        {
            countdown1Done = true; // flag the countdown 1 as done
            countdownImgAlpha = 1.0f; // set the countdown image alpha to 1
            countdownImg.sprite = countdown1Sprite; // set the countdown image sprite to the countdown 1 sprite
            finishArchAudioClipPlayer.PlayOneShot(0, 3, 1.0f, true); // play the countdown horn
            TweenCountdownSprite(); // tween the countdown sprite
            hintsWindow.TriggerRandomHint(); // trigger a random hint
        }
        else if (!countdownGoDone && raceController.raceStartCountdownTimer == 0f) // check if the countdown go is not done and the race start countdown timer is equal to zero
        {
            countdownGoDone = true; // flag the countdown go as done
            countdownImgAlpha = 1.0f; // set the countdown image alpha to 1
            countdownImg.sprite = countdownGoSprite; // set the countdown image sprite to the countdown go sprite
            finishArchAudioClipPlayer.PlayOneShot(0, 2, 1.0f, true); // play the final countdown horn
            TweenCountdownSprite(); // tween the countdown sprite
        }
        countdownImg.color = new Color(1f, 1f, 1f, countdownImgAlpha); // update the countdown image color alpha
    }

    public void TweenCountdownSprite()
    {
        Sequence sequence = DOTween.Sequence(); // generate a new dotween sequence
        sequence.Append(countdownImg.transform.DOMove(countdownDownAnchor.transform.position, .4f)).SetEase(Ease.OutBounce,0.1f); // append to the sequence a tween that animates the countdown image position to the countdown anchor position in .4 seconds
        sequence.Append(countdownImg.transform.DOMove(countdownUpAnchor.transform.position, .4f)); // append to the sequence a tween that animates the countdown image position to the countdown up anchor in .4 seconds
        Sequence sequence2 = DOTween.Sequence(); // generate a new dotween sequence
        sequence2.Append(DOTween.To(() => countdownImgAlpha, (newValue) => countdownImgAlpha = newValue, 1f, .3f)); // append to the sequence a tween that animates the countdown image alpha from 0 to 1 in .3 seconds
        sequence2.Append(DOTween.To(() => countdownImgAlpha, (newValue) => countdownImgAlpha = newValue, 0f, .1f)); // append to the sequenca a tween that animates the countdown image alpha from 1 to 0 in .1 seconds
    }

    public void UpdateMutedStatus()
    {
        mixerMain.UpdateMutedStatus(); // update the game's mixer muted status
    }

    public void ToggleMutedStatus()
    {
        mixerMain.ToggleMutedStatus(); // toggle the game's mixer muted status
    }

    public void TweenPressToContinueText()
    {
        DOTween.To(() => pressToContinueTextAlpha, (newValue) => pressToContinueTextAlpha = newValue, 0f, 600f).SetEase(Ease.InOutFlash, 300f, 0f); // tween the press to continue text alpha as an oscillation
        pressToContinueText.rectTransform.DOScale(new Vector3(pressToContinueTextStartScaleX + 0.1f, pressToContinueTextStartScaleY + 0.1f, 1f), 1.0f).SetLoops(50, LoopType.Yoyo); // tween the press to continue scale as an oscillation 
    }

    internal void TweenCoinText()
    {
        Sequence sequence = DOTween.Sequence(); // generate new dotween sequence
        sequence.Append(coinsText.rectTransform.DOScale(new Vector3(coinTextStartScaleX + 0.1f, coinsTextStartScaleY + 0.1f, 1f), 0.2f)); // append to the sequence a tween that animates the coin text scale up in .2 seconds
        sequence.Append(coinsText.rectTransform.DOScale(new Vector3(coinTextStartScaleX, coinsTextStartScaleY, 1f), 0.1f)); // append to the sequence a tween that animates the animates the coint text down i .1 seconds
    }

    internal void TweenSpeedupsText()
    {
        Sequence sequence = DOTween.Sequence(); // generate new dotween sequence
        sequence.Append(usableSpeedsupText.rectTransform.DOScale(new Vector3(usableSpeedupsTextStartScaleX + 0.1f, usableSpeedupsTextStartScaleY + 0.1f, 1f), 0.2f)); // append to the sequence a tween that animates the usable speedups text up in .2 seconds
        sequence.Append(usableSpeedsupText.rectTransform.DOScale(new Vector3(usableSpeedupsTextStartScaleX, usableSpeedupsTextStartScaleY, 1f), 0.1f)); // append to the sequence a tween that animates the animates the usable speedups text down in .1 seconds
    }

    public void UpdateRetroFilterStatus()
    {
        globalVolume.gameObject.SetActive(userSettings.retroFilterOn); // update the activity status of the game's global volume
    }

    public void ToggleRetroFilterStatus()
    {
        userSettings.retroFilterOn = !userSettings.retroFilterOn; // toggle the user's settings retro filter activity status
        PlayerPrefs.SetFloat("RetroFilterOn?", userSettings.retroFilterOn ? 1 : 0); // update the player prefs with the new retro filter activity status
        PlayerPrefs.Save(); // save the player prefs
        globalVolume.gameObject.SetActive(userSettings.retroFilterOn); // update the activity status of the game's global volume
    }
}
