using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Video;

public class UIHintsWindow : MonoBehaviour
{
    public bool active = false; // the status of the ui hints window
    public Sprite[] sprites; // the array of possible sprites toe be displayed
    public VideoClip[] videoClips;  // the array of possible video clips to be displayed
    public Image hintDisplay0; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay1; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay2; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay3; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay4; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay5; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay6; // an image that display hints (all images are overlayed on top of each other)
    public Image hintDisplay7; // an image that display hints (all images are overlayed on top of each other)
    public TextMeshProUGUI hintText0; // a text that displays hints
    public TextMeshProUGUI hintText1; // a text that displays hints
    public VideoPlayer videoHintDisplay0; // a video player that displays hints
    public GameObject videoBorder; // the object of the border image of the video
    public UserSettings userSettings; // the user's settings
    private void Start()
    {
        userSettings = FindObjectOfType<UserSettings>(); // store a local reference to the user's settings
        ClearHintsWindow(); // clear the hints window back to its inactive (hidden) state
    }

    public void ClearHintsWindow()
    {
        active = false; // flag the hints window as inactive
        float normalScaleMagnitude = 1.8f; // the constant for normal scale magnitude for all image hint displays
        hintDisplay0.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay0.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay1.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay1.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay2.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay2.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay3.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay3.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay4.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay4.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay5.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay5.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay6.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay6.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintDisplay7.sprite = sprites[0]; // set the sprite to the first element of the sprites array (fully transparent image)
        hintDisplay7.rectTransform.localScale = new Vector3(normalScaleMagnitude, normalScaleMagnitude, 1.0f); // set the local scale to the normal scale magnitude
        hintText0.text = ""; // set the hints text to an empty string
        hintText1.text = ""; // set the hints text to an empty string
        videoHintDisplay0.clip = null; // set the video clip to null
        videoHintDisplay0.gameObject.SetActive(false); // disable the video hints display
        videoBorder.gameObject.SetActive(false); // disable the video's border
    }

    public void TriggerRandomHint()
    {
        if (!userSettings.hintsAndTips) { return; } // return if the has disable hints and tips 
        active = true; // flag the hints window as active
        int numberOfPossibleHints = 8; // the number of possible hints 
        int randomHintID = (int) UnityEngine.Random.Range(0f, numberOfPossibleHints); // generate a random hint id out of the possible nubmer of hints
        float punchScaleMagnitude = .25f; // the punch scale magnitude
        float punchScaleDurationSeconds = 1.0f; // the punch scale duration
        int punchScaleVibrato = 1; // the punch scale vibrato
        float punchScaleElasticity = .1f; // the punch scale elasticity
        float hintDurationSeconds = 10.0f; // the hint duration in seconds
        int numberOfLoops = (int)((hintDurationSeconds / punchScaleDurationSeconds) - 1); // calculate the number of times the punch scale should loop
        switch (randomHintID) // random hint id fork
        {
            case 0: // hint 0 (THRUST FORWARD)
                hintDisplay0.sprite = sprites[1]; // display the W key image
                hintDisplay1.sprite = sprites[2]; // display the A key image
                hintDisplay2.sprite = sprites[3]; // display the S key image
                hintDisplay3.sprite = sprites[4]; // display the D key image
                hintDisplay0.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the W key
                hintDisplay4.sprite = sprites[5]; // display the UP key image
                hintDisplay5.sprite = sprites[6]; // display the LEFT key image
                hintDisplay6.sprite = sprites[7]; // display the DOWN key image
                hintDisplay7.sprite = sprites[8]; // display the RIGHT key image
                hintDisplay4.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the UP key
                hintText0.text = "Press W or UP to thrust forward"; // set the hints text
                videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[0]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 1: // hint 1 (STEER LEFT)
                hintDisplay0.sprite = sprites[1]; // display the W key image
                hintDisplay1.sprite = sprites[2]; // display the A key image
                hintDisplay2.sprite = sprites[3]; // display the S key image
                hintDisplay3.sprite = sprites[4]; // display the D key image
                hintDisplay1.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the A key
                hintDisplay4.sprite = sprites[5]; // display the UP key image
                hintDisplay5.sprite = sprites[6]; // display the LEFT key image
                hintDisplay6.sprite = sprites[7]; // display the DOWN key image
                hintDisplay7.sprite = sprites[8]; // display the RIGHT key image
                hintDisplay5.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the LEFT key
                hintText0.text = "Press A or LEFT to steer to the left"; // set the hints text
                videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[1]; // set the video clip
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 2: // hint 2 (THRUST REVERSE)
                hintDisplay0.sprite = sprites[1]; // display the W key image
                hintDisplay1.sprite = sprites[2]; // display the A key image
                hintDisplay2.sprite = sprites[3]; // display the S key image
                hintDisplay3.sprite = sprites[4]; // display the D key image
                hintDisplay2.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the S key
                hintDisplay4.sprite = sprites[5]; // display the UP key image
                hintDisplay5.sprite = sprites[6]; // display the LEFT key image
                hintDisplay6.sprite = sprites[7]; // display the DOWN key image
                hintDisplay7.sprite = sprites[8]; // display the RIGHT key image
                hintDisplay6.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the DOWN key
                hintText0.text = "Press S or DOWN to go in reverse"; // set the hints text
               videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[2]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 3: // hint 3 (STEER RIGHT)
                hintDisplay0.sprite = sprites[1]; // display the W key image
                hintDisplay1.sprite = sprites[2]; // display the A key image
                hintDisplay2.sprite = sprites[3]; // display the S key image
                hintDisplay3.sprite = sprites[4]; // display the D key image
                hintDisplay3.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the D key
                hintDisplay4.sprite = sprites[5]; // display the UP key image
                hintDisplay5.sprite = sprites[6]; // display the LEFT key image
                hintDisplay6.sprite = sprites[7]; // display the DOWN key image
                hintDisplay7.sprite = sprites[8]; // display the RIGHT key image
                hintDisplay7.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), punchScaleDurationSeconds, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops); // tween the RIGHT key
                hintText0.text = "Press D or RIGHT to steer to the right"; // set the hints text
                videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[3]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 4: // hint 4 (BRAKE)
                hintDisplay0.sprite = sprites[9];
                hintDisplay0.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), 1.0f, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops);
                hintText1.text = "Press SPACE to brake"; // set the hints text
                videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[4]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 5: // hint 5 (DRIFT)
                hintDisplay0.sprite = sprites[10];
                hintDisplay0.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                hintDisplay0.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), 1.0f, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops);
                hintText1.text = "Press CTRL to enter a drift"; // set the hints text
                videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[5]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 6: // hint 6 (TOGGLE HEADLIGHTS)
                hintDisplay0.sprite = sprites[11];
                hintDisplay0.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                hintDisplay0.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), 1.0f, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops);
                hintText1.text = "Press L to toggle the headlights"; // set the hints text
               videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
               videoHintDisplay0.clip = videoClips[6]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
            case 7: // hint 7 (PAUSE GAME)
                hintDisplay0.sprite = sprites[12];
                hintDisplay0.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                hintDisplay0.rectTransform.DOPunchScale(new Vector3(punchScaleMagnitude, punchScaleMagnitude, 1.0f), 1.0f, punchScaleVibrato, punchScaleElasticity).SetLoops(numberOfLoops);
                hintText1.text = "Press ESC to pause the game"; // set the hints text
               videoHintDisplay0.gameObject.SetActive(true); // activate the video hints display
                videoHintDisplay0.clip = videoClips[7]; // set the video clip 
                videoBorder.gameObject.SetActive(true); // activate the video border
                break;
        }
        StartCoroutine(ResetHintsWindowAfterSeconds(hintDurationSeconds)); // start the couroutine which cleans and resets the hints window after hintDurationSeconds seconds
    }
    public IEnumerator ResetHintsWindowAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds); // wait seconds seconds
        ClearHintsWindow(); // clear and reset the hints window
    }

}
