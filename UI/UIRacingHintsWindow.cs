using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIRacingHintsWindow : MonoBehaviour
{
    public bool active = false; // the [on off] state of the racing hints window. if active, the window is displaying its information, if inactive, its hidden.
    public Sprite[] sprites; // the array of all possible sprites to be displayed
    public Image racingHintDisplay; // the image, in which sprites are to be displayed
    public float displayAlpha = 255f; // the transparency alpha of the display (starts fully visible)
    public Tween alphaTween; // the tween to handle the alpha transparency changes of the display (oscillates from 255 transparency to 0 transparency)
    public UserSettings userSettings;

    private void Start()
    {
        userSettings = FindObjectOfType<UserSettings>();
        ClearHintsWindow(); // reset the hints window back to its hidden state
    }

    public void ClearHintsWindow()
    {
        active = false; // the window will go into its hidden state, so set it inactive
        racingHintDisplay.sprite = sprites[0]; // set the sprite to the first sprite in the array, which is a fully transparent image (warning: never change the order of the array!)
        alphaTween.Kill(); // kill the window tween
        displayAlpha = 255f; // set the alpha transparency back to fully visible
    }

    public void TriggerRacingHint(int hintID)
    {

        if (!userSettings.hintsAndTips) // dont trigger the racing hint if the player has disabled hints and tips
        {
            return;
        }
        active = true; // the window will go into its information displaying state, so set it active

        float hintDurationSeconds = 3.0f; // set the constant of the hints duration

        switch (hintID) // hint ID fork
        {
            case 0:
                racingHintDisplay.sprite = sprites[1]; // Sharp right
                break;
            case 1:
                racingHintDisplay.sprite = sprites[2]; // Sharp left
                break;
            case 2:
                racingHintDisplay.sprite = sprites[3]; // Turn right
                break;
            case 3:
                racingHintDisplay.sprite = sprites[4]; // Turn left
                break;
            case 4:
                racingHintDisplay.sprite = sprites[5]; // Y splitter
                break;
            case 5:
                racingHintDisplay.sprite = sprites[6]; // Straight ahead
                break;
            case 6:
                racingHintDisplay.sprite = sprites[7]; // Warning 
                break;
            case 7:
                racingHintDisplay.sprite = sprites[8]; // Slow down
                break;
        }
        TweenDisplay(); // tween the display (oscilation of the alpha transparency from [255f to 0f]
        StartCoroutine(ResetHintsWindowAfterSeconds(hintDurationSeconds)); // start the couroutine which resets the window back to its hidden state after a certain amount of seconds
    }

    private void Update()
    {
        racingHintDisplay.color = new Color32(255, 255, 255, (byte)displayAlpha); // every frame, set the the alpha transparency of the image
    }

    public IEnumerator ResetHintsWindowAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds); // wait for a certain amount of seconds
        ClearHintsWindow(); // then clear the hints window

    }

    public void TweenDisplay()
    {
        alphaTween =  DOTween.To(() => displayAlpha, (newValue) => displayAlpha = newValue, 0f, 255f).SetEase(Ease.InOutFlash, 300f, 0f); // oscillate the display alpha variable from 255f to 0f

    }
}

