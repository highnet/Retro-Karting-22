using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIImageGrow : MonoBehaviour
{
    private Image image; // the ui image to grow
    private float startScaleX; // the starting scale x axis magnitude
    private float startScaleY; // the starting scale y axis magnitude
    public float growDuration; // the duration it takes to grow from [0 0 0] to [startScaleX startScale 1]

    void Start()
    {
        image = GetComponent<Image>(); // store a local reference to the image
        startScaleX = image.transform.localScale.x; // store the starting scale x magnitude
        startScaleY = image.transform.localScale.y; // store the starting scale y magnitude
        image.rectTransform.localScale = Vector3.zero; // set the image rect transform local scale to 0 0 0
        GrowImageToOriginalSize(); // grow the image back to its starting size
    }

    public void GrowImageToOriginalSize()
    {
        image.rectTransform.DOScale(new Vector3(startScaleX, startScaleY, 1f), growDuration).SetEase(Ease.InQuad); // tween the image rect transform scale from [0 0 0] to [startScaleX startScale 1]
    }


}
