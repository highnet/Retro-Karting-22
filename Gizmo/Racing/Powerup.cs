using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Powerup : MonoBehaviour
{
    public KartController kartController; // the kart controller
    public float speedUpStrength; // the strength of the speedup powerup
    public float megaSpeedUpStrength; // the strength of the mega speedup powerup
    public float coinSpeedStrength; // the strength of the coin speed powerup
    public RacingUIController racingUIController; // the racing ui controller

    private void Start()
    {
        kartController = GameObject.FindGameObjectWithTag("Controller").GetComponent<KartController>(); // store a local reference to the kart controller
        racingUIController = FindObjectOfType<RacingUIController>(); // store a local reference to the racing ui controller
    }

    public void DoIncreaseUsableSpeedups()
    {
        kartController.characterClipPlayer.PlayOneShot(0, 2, 1.0f, true); // play the sound
        kartController.usableSpeedups++; // increase the kart's usable speedups
        racingUIController.usableSpeedsupText.text = kartController.usableSpeedups.ToString(); // update the usable speedups text to reflect the increase in usable speedups
        racingUIController.TweenSpeedupsText(); // tween the usable speedups text in the UI
    }

    public void DoMegaSpeedUp()
    {
        kartController.characterClipPlayer.PlayOneShot(0, 2, 1.0f, true); // play the sound
        kartController.SpeedBoost(megaSpeedUpStrength, Color.green, 1.0f, 1.0f, 1.0f, 20f,true,true, ForceMode.Acceleration); // apply the speed boost to the kart
    }

    internal void DoIncreaseCoinCount()
    {
        kartController.characterClipPlayer.PlayOneShot(0, 3, 1.0f, false); // play the sound
        if (kartController.coinCount < 10) // if the kart has less than 10 coins
        {
            kartController.coinCount++; // increment the coin count
            racingUIController.coinsText.text = kartController.coinCount.ToString(); // update the coins text to reflect the increase in coins
            racingUIController.TweenCoinText(); // tween the coins text in the UI
        }
    }

    public void DoSpeedUp()
    {
        kartController.SpeedBoost(speedUpStrength,Color.green,1.0f,1.0f,1.0f,20f,true,true, ForceMode.Acceleration); // apply the speedboost to the kart
    }
}
