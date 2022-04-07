using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIAnimatePickTracks : MonoBehaviour
{
    public List<Image> borders; // the list of images to be animated
    public UIPanelMovement uiPanelMovement; // the ui panel movement script

    public void SlideBordersDelayed(float s)
    {
        uiPanelMovement.busyAnimating = true; // flag the ui panel movement script as busy animating
        for (int i = 0; i < borders.Count; i++) // for each border
        {
            borders[i].rectTransform.Translate(new Vector3(0f, 3000f, 0f)); // move the border off screen
        }
        StartCoroutine(SlideBordersAfterSeconds(s)); // start the co routie whicn slides the border into place after s seconds
    }

    public IEnumerator SlideBordersAfterSeconds(float s)
    {
        yield return new WaitForSeconds(s); // wait for s seconds
        int i; // loop iteration variable
        for (i = 0; i < borders.Count-1; i++) // for each border except the last one
        {
            borders[i].rectTransform.DOMove(borders[i].GetComponent<UIPickTrack>().anchor.position, .33f + (i * 0.1f)).SetEase(Ease.InOutBounce); // move each border to its anchor position in .33f + (i * 0.1f) seconds, each border is delayed a bit more to animate a cascading anchoring effect
        }
        

        borders[borders.Count-1].rectTransform.DOMove(borders[borders.Count-1].GetComponent<UIPickTrack>().anchor.position, .33f + (i * 0.1f)).SetEase(Ease.InOutBounce) // move the last border to its anchor position in in .33f + (i * 0.1f) seconds ...
            .OnComplete(() => { // ... and when its over...
                EndBusyAnimating(); // ... flag the ui panel movement script as not busy animating
            });
        

    }

    public void EndBusyAnimating()
    {
        uiPanelMovement.busyAnimating = false; // flag the ui panel movement script as not busy animating

    }
}
