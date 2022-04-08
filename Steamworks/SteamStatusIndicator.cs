using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class SteamStatusIndicator : MonoBehaviour
{
    private Steam steam; // the steam script
    public TextMeshProUGUI steamName; // the text that display's the steam name
    public Image steamImage; // the image that displays the steam image
    public GameObject anchor0; // the status indicator positional achor 0
    public GameObject anchor1; // the status indicator positional achor 1
    public GameObject anchor2; // the status indicator positional achor 2
    public Sequence movementSequence; // the dotween movement sequence

    public void DoMovementSequence()
    {
        steam = FindObjectOfType<Steam>(); // store a local reference to the steam script
        if (steam.GetSteamInitialized()) // if steam is initialized
        {
            steamName.text = "Hello, " + steam.GetSteamName(); // update the text
            steamImage.sprite = steam.GetSteamAvatar(); // update the image
        }
        movementSequence = DOTween.Sequence(); // create a new sequence
        movementSequence.Append(this.transform.DOMove(anchor0.transform.position, .9f).SetEase(Ease.OutBounce)); // append to the sequence a tween that moves the status indicator to the anchor 0 positoin in .9 seconds
        movementSequence.AppendInterval(.33f);  // append to the sequence a .33 second interval
        movementSequence.Append(this.transform.DOMove(anchor1.transform.position, 0.15f).SetEase(Ease.InOutBounce)); // append to the sequence a tween that moves the status indicator to the anchor 0 positoin in .9 seconds
        movementSequence.Append(this.transform.DOMove(anchor2.transform.position, 0.3f).SetEase(Ease.OutQuad)); // append to the sequence a tween that moves the status indicator to the anchor 0 positoin in .9 seconds
    }
}
