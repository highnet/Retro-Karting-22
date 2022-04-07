using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class SteamStatusIndicator : MonoBehaviour
{

    private Steam steam;
    public TextMeshProUGUI steamName;
    public Image steamImage;
    public GameObject anchor0;
    public GameObject anchor1;
    public GameObject anchor2;

    public Sequence movementSequence;

    public void DoMovementSequence()
    {
        steam = FindObjectOfType<Steam>();

        if (steam.GetSteamInitialized())
        {
            steamName.text = "Hello, " + steam.GetSteamName();
            steamImage.sprite = steam.GetSteamAvatar();
        }
        movementSequence = DOTween.Sequence();
        movementSequence.Append(this.transform.DOMove(anchor0.transform.position, .9f).SetEase(Ease.OutBounce)); 
        movementSequence.AppendInterval(.33f);
        movementSequence.Append(this.transform.DOMove(anchor1.transform.position, 0.15f).SetEase(Ease.InOutBounce));
        movementSequence.Append(this.transform.DOMove(anchor2.transform.position, 0.3f).SetEase(Ease.OutQuad));
    }
}
