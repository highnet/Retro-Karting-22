using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIDetectClick : MonoBehaviour
{
    public IntroUIController introUIController; // the intro ui controller script

    public void IncrementNumberOfClicks()
    {
        introUIController.numberOfClicks++; // increment the number of times the user clicked the intro ui
    }
}

