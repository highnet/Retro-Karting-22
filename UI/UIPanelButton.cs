using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class UIPanelButton : MonoBehaviour
{
    public MainMenuUIController mainMenuUIController; // the main menu ui controller
    public Image image; // the image of the button
    public float currentImageAlpha; // the current image alpha
    public float minImageAlpha; // the minimum image alpha when its faded out
    public float maxImageAlpha; // the maximum imahe alpha wheni ts faded in
    public float alphaFadeInTime; // the time it takes to fade in the button
    public float alphaFadeOutTime; // the time it takes to fade out the button
    private float startScaleX; // the button's x scale at the start
    private float startScaleY; // the button's y scale at the start
    public bool growAtStart; // flag to indicate wether the button should grow at Start()
    public float growAtStartDuration; // the duration in seconds it takes to grow the button
    public UIPanelMovement currentPanel; // the current panel in which the button resides in
    public UIPanelMovement destinationPanel; // the destination panel that the button leads you to when you activate it

    private void Start()
    {
        image = GetComponent<Image>(); // store a local reference to the image
        currentImageAlpha = minImageAlpha; // set the current image alpha to the minimum image alpha
        startScaleX = image.transform.localScale.x; // save the start scale x from the image transform local scale
        startScaleY = image.transform.localScale.y; // save the start scale y from the iamge transform local scale
        if (growAtStart) // check if the button should grow at Start()
        {
            image.transform.localScale = Vector3.zero; // set the image transform local scale to zero so we can grow it again
            image.rectTransform.DOScale(new Vector3(startScaleX, startScaleY, 1f), growAtStartDuration).SetEase(Ease.InQuad); // tween the image's scale from (0 0 0) to (starscaleX startScaleY 1) in growAtStartDuration seconds
        }
    }

    public void FadeImageIn()
    {
        DOTween.To(() => currentImageAlpha, (newValue) => currentImageAlpha = newValue, maxImageAlpha, alphaFadeInTime).SetEase(Ease.InQuad); // fade the image alpha out from the current image alpha to the maximum image alpha in alphaFAdeInTime seconds
        image.rectTransform.DOScale(new Vector3(startScaleX + 0.05f, startScaleY + 0.025f, 1f), alphaFadeInTime).SetEase(Ease.InQuad); // scale the image up
    }

    public void FadeImageOut()
    {
        DOTween.To(() => currentImageAlpha, (newValue) => currentImageAlpha = newValue, minImageAlpha, alphaFadeOutTime).SetEase(Ease.InQuad); // fade the image alpha in from the current image alpha to the minimum image alpha in alphaFadeOutTime seconds
        image.rectTransform.DOScale(new Vector3(startScaleX, startScaleY, 1f), alphaFadeOutTime).SetEase(Ease.InQuad); // scale the image down to its start scale
    }

    public void ChangePanel()
    {
        if (!currentPanel.interactable || currentPanel.busyAnimating) { return; } // return if the panel current panel is not interactable or its busy animating
        currentPanel.interactable = false; // flag the current panel as not interactable
        destinationPanel.interactable = true; // flag the destination panel as interactable
        Sequence sequence2 = DOTween.Sequence(); // generate a new sequence
        sequence2.Append(currentPanel.transform.DOMove(currentPanel.offscreenAnchor.transform.position, .66f).SetEase(Ease.InOutQuad)); // append to the sequence a tween that moves the current panel to its offscreen position in .66 seconds
        sequence2.Append(destinationPanel.transform.DOMove(destinationPanel.centerAnchor.transform.position, .66f).SetEase(Ease.InOutQuad)); // append to the sequence a tween that moves the destination panel to its center anchor in .66 seconds
        Sequence sequence = DOTween.Sequence(); // generate a new sequence
        sequence.Append(DOTween.To(() => mainMenuUIController.currentBackgroundImageAlpha, (newValue) => mainMenuUIController.currentBackgroundImageAlpha = newValue, 0, .66f).SetEase(Ease.OutQuad)); // append to the sequence a tween that fades the background image alpha out
        sequence.Append(DOTween.To(() => mainMenuUIController.currentBackgroundImageAlpha, (newValue) => mainMenuUIController.currentBackgroundImageAlpha = newValue, mainMenuUIController.startBackgroundImageAlpha, .66f).SetEase(Ease.InQuad)); // append to the sequence a tween that fades the background image alpha in
    }

    public void GoBackThenChangePanel()
    {
        if (!currentPanel.interactable || currentPanel.busyAnimating) { return; } // return if the current panel is not interactable or its busy animating
        currentPanel.interactable = false; // flag the current panel as not interactable
        destinationPanel.interactable = true; // flag the destination panel as interactable 
        Sequence sequence2 = DOTween.Sequence(); // generate a new sequence
        sequence2.Append(currentPanel.transform.DOMove(currentPanel.offscreenAnchor.transform.position, .33f)); // append to the sequence a tween that moves the current panel to its offscreen position in .33 seconds
        sequence2.Append(mainMenuUIController.masterPanel.transform.DOMove(destinationPanel.centerAnchor.transform.position, .33f)); // append to the sequence a tween that moves the master panel to the destination panel's center anchor in .33 seconds
        sequence2.Append(mainMenuUIController.masterPanel.transform.DOMove(mainMenuUIController.masterPanel.offscreenAnchor.transform.position, .33f)); // append to the sequence a tween that moves the master panel to the master panel's offscreen anchor in .33 seconds
        sequence2.Append(destinationPanel.transform.transform.DOMove(destinationPanel.centerAnchor.transform.position, .33f)); // append to the sequence a tween that moves the destination panel to its center anchor in .33 seconds
        Sequence sequence = DOTween.Sequence();// generate a new sequence
        sequence.Append(DOTween.To(() => mainMenuUIController.currentBackgroundImageAlpha, (newValue) => mainMenuUIController.currentBackgroundImageAlpha = newValue, 0, .66f).SetEase(Ease.OutQuad)); // append to the sequence a tween that fades the background image alpha out
        sequence.Append(DOTween.To(() => mainMenuUIController.currentBackgroundImageAlpha, (newValue) => mainMenuUIController.currentBackgroundImageAlpha = newValue, mainMenuUIController.startBackgroundImageAlpha, .66f).SetEase(Ease.InQuad));  // append to the sequence a tween that fades the background image alpha in

    }

    public void QuitButton()
    {
        Debug.Log("Bye Bye!"); // debug for editor mode since, Application.Quit(); doesnt do anything in editor mode
        Application.Quit(); // Quit the application in build mode
    }


    private void Update()
    {
        image.color = new Color32(255, 255, 255, (byte) currentImageAlpha); // set the button's image color alpha
    }
}
