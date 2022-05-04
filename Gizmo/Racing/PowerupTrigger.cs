using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTrigger : MonoBehaviour
{
    public enum PowerupType {None, Speedup, MegaSpeedup, CollectibleSpeedup, Coin}; // the types of powerups
    public PowerupType powerupType; // the type of powerup
    public AudioClipPlayer audioClipPlayer; // the audio clip player
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // check the collision tag for "Player"
        {
            KartController kartController = GameObject.FindGameObjectWithTag("Controller").GetComponent<KartController>(); // get the kart controller
            Powerup powerup = kartController.powerup; // get the kart controller's powerup script
            audioClipPlayer.PlayOneShot(1, 1, 1.0f, true); // play the powerup trigger sound
            if (powerupType == PowerupType.Speedup) // speedup powerup
            {
                powerup.DoSpeedUp(); // apply the speed up boost
            }
            else if (powerupType == PowerupType.MegaSpeedup) // mega speedup powerup
            {
                powerup.DoMegaSpeedUp(); // apply the mega speedup boost
            }
            else if (powerupType == PowerupType.CollectibleSpeedup) // collectible speedup powerup
            {
                powerup.DoIncreaseUsableSpeedups(); // increase the usable speedup boost
            }
            else if(powerupType == PowerupType.Coin) // coin powerup
            {
                powerup.DoIncreaseCoinCount(); //increase the coin counter
            } 
            this.transform.Translate(new Vector3(1000f, 1000f, 1000f)); // move the powerup trigger off map
        }
    }
}
