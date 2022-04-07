using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public KartController kartController; // the kart controller
    public float maxSpeedYellCooldown; // the max speed yell cooldown
    public float maxSpeedYellTimer; // the max speed yell timer
    public float crashYellCooldown; // the crash yell cooldown
    public float crashYellTimer; // the crash yell timer
    public float randomLaughYellRandomCooldownMinimum; // the random laugh yell random cooldown minimum
    public float randomLaughYellRandomCooldownMaximum; // the random laugh yell random cooldown maximum
    public float randomLaughYellTimer; // the random laugh yell timer
    private void Start()
    {
        kartController = FindObjectOfType<KartController>(); // store a local reference to the kart controller
    }

    private void FixedUpdate()
    {
        if (maxSpeedYellTimer > 0) // if the max speed yell timer larger than 0 
        {
            maxSpeedYellTimer -= Time.deltaTime; // deduct time from it
            if (maxSpeedYellTimer < 0) // if it has gone below zero
            {
                maxSpeedYellTimer = 0; // set it to zero
            }
        }
        if (crashYellTimer > 0) // if the crash yell timer is larger than 0
        {
            crashYellTimer -= Time.deltaTime; // deduct time from it
            if (crashYellTimer < 0) // if it has gone below zero
            {
                crashYellTimer = 0; // set it to zero
            }
        }
        if (randomLaughYellTimer > 0) // if the random laugh yell timer is larger than 0
        {
            randomLaughYellTimer -= Time.deltaTime; // deduct time from it
            if (randomLaughYellTimer < 0) // if it has gone below zero
            {
                randomLaughYellTimer = 0; // set it to zero
            }
        }
        if (maxSpeedYellTimer == 0 && kartController.currentThrustForce >= kartController.currentMaxThrustForce - 1) // if the timer has reached zero (off cooldown) and the kart's thrust force is near its maximum thrust force
        {
            kartController.characterClipPlayer.PlayOneShot(0, 1, 1.0f, true); // play the max speed yell sound
            maxSpeedYellTimer = maxSpeedYellCooldown; // set the timer back on cooldown
        }
        if (randomLaughYellTimer == 0) // if the random laugh yell timer has reached zero (off cooldown)
        {
            kartController.characterClipPlayer.PlayOneShot(0, 6, 1.0f, true); // play the laugh yell sound
            randomLaughYellTimer = UnityEngine.Random.Range(randomLaughYellRandomCooldownMinimum,randomLaughYellRandomCooldownMaximum); // set the timer back on cooldown in a random number ranging from [randomLaughYellRandomCooldownMinimum to randomLaughYellRandomCooldownMaximum] 
        }
    }

    public void StartEngine()
    {
        kartController.kartClipPlayer.PlayOneShot(0, 23, 1.0f, true); // play the start engine sound
    }

    public void SayAreYouReady()
    {
        kartController.characterClipPlayer.PlayOneShot(0, 0, 1.0f, true); // play the are you ready sound
    }
}
