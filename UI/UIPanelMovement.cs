using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPanelMovement : MonoBehaviour
{
    private GameObject panel; // the panel to move
    public GameObject centerAnchor; // the center anchor of the panel to move
    public GameObject offscreenAnchor; // the offscreen anchor of the panel to move
    public float centerDuration; // the duration it takes to move the panel to the center anchor
    public bool centerPanelAtStart; // if true, the panel will move to its center anchor at Start()
    public bool interactable; // flag for status of wether or not the panel is interactable or not
    public bool busyAnimating; // flag for status of wether or not the panel is currently busy animating or not
    public SteamStatusIndicator steamStatusIndicator; // the steam status indicator
    // Start is called before the first frame update
    void Start()
    {
        panel = this.gameObject; // store a local reference to the panel
        if (centerPanelAtStart) // check if the panel should get centered at Start()
        {
            CenterPanel(); // center the panel
        }
    }

    public void CenterPanel()
    {
        panel.transform.DOMove(centerAnchor.transform.position, centerDuration).SetEase(Ease.InOutBounce) // tween the panel's position to the center anchor position in centerDuration seconds
            .OnComplete(() => { // ... and when its complete ...
                if (steamStatusIndicator) // ... if steamstatusindicator exists ...
                {
                    steamStatusIndicator.gameObject.SetActive(true); // ... activate the steam status indicator ...
                    steamStatusIndicator.DoMovementSequence(); // ... and do the steam status indicator movement sequence 
                }
            });

    }


}
