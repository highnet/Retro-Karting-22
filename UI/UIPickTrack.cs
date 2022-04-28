using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static GameStateManager;

public class UIPickTrack : MonoBehaviour
{
    public Image border; // the pick track element's border
    public float startingColorR; // the border's starting color red
    public float startingColorG; // the border's starting color green
    public float startingColorB; // the border's starting color blue
    public float startingColorA; // the border's starting color alpha
    public float tweenColorR; // the border's color red once its highlighted (after tweening)
    public float tweenColorG; // the border's color green once its highlighted (after tweening)
    public float tweenColorB; // the border's color blue once its highlighted (after tweening)
    public float tweenColorA; // the border's color alpha once its highlighted (after tweening)
    public float highlightTweenTime; // the time to highlight the border's color from the starting color to the end color
    private float currentColorR; // the border's current color red
    private float currentColorG; // the border's current color green
    private float currentColorB; // the border's current color blue
    private float currentColorA; // the border's current color alpha
    private SceneManager sceneManager; // the game's scene manager
    public int possibleNumberOfTracks; // the possible number of tracks in the game
    public RectTransform anchor; // the positional anchor of the UI pick track element
    public UIPanelMovement panel; // the handler for moving the UI pick track element

    // Start is called before the first frame update
    void Start()
    {
        border = GetComponent<Image>(); // store a local reference to the border image
        currentColorR = startingColorR; // set the current color red to the starting color red
        currentColorG = startingColorG; // set the current color green to the starting color green
        currentColorB = startingColorB; // set the current color blue to the starting color blue
        currentColorA = startingColorA; // set the current color alpha to the starting color alpha
        sceneManager = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManager>(); // store a local reference to the game's scene manager
    }

    private void Update()
    {
        border.color = new Color32((byte) currentColorR, (byte) currentColorG, (byte) currentColorB, (byte) currentColorA); // set the border's colors with the current colors
    }

    public void HighlightBorderOn()
    {
        DOTween.To(() => currentColorR, (newValue) => currentColorR = newValue, tweenColorR, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color red value to the tween color red value in highlightTweenTime seconds 
        DOTween.To(() => currentColorG, (newValue) => currentColorG = newValue, tweenColorG, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color green value to the tween color green value in highlightTweenTime seconds
        DOTween.To(() => currentColorB, (newValue) => currentColorB = newValue, tweenColorB, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color blue value to the tween color blue value in highlightTweenTime seconds
        DOTween.To(() => currentColorA, (newValue) => currentColorA = newValue, tweenColorA, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color alpha value to the tween color alpha value in highlightTweenTime seconds
    }

    public void HighlightBorderOff()
    {
        DOTween.To(() => currentColorR, (newValue) => currentColorR = newValue, startingColorR, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color red value to the starting color red value in highlightTweenTime seconds
        DOTween.To(() => currentColorG, (newValue) => currentColorG = newValue, startingColorG, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color green value to the starting color green value in highlightTweenTime seconds
        DOTween.To(() => currentColorB, (newValue) => currentColorB = newValue, startingColorB, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color blue value to the starting color blue value in highlightTweenTime seconds
        DOTween.To(() => currentColorA, (newValue) => currentColorA = newValue, startingColorA, highlightTweenTime).SetEase(Ease.InQuad); // tween the current color alpha value to the starting color alpha value in highlightTweenTime seconds
    }

    public void PickRandomTrack()
    {
        if (!panel.interactable) // check if the panel is flagged as not interactable
        {
            return; // return so we dont pick a track
        }
        int randomTrackNumber = UnityEngine.Random.Range(1, possibleNumberOfTracks+1); // generate a random track number id
        if (sceneManager != null) // if the scene manager exists
        {
            sceneManager.ChangeScene("Racing Scene " + randomTrackNumber,GameState.Racing,GameMode.TimeTrial); // change scene to the random racing scene

        }
    }

    public void PickTrack(string sceneName)
    {
        if (!panel.interactable) // check if the panel is flagged as not interactable
        {
            return; // return so we dont pick a track
        }
        if (sceneManager != null) // if the scene manager exists
        {
            sceneManager.ChangeScene(sceneName,GameState.Racing,GameMode.TimeTrial); // change to the appropiate scene in time trial mode
        }
    }
}
