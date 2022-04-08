using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartCollision : MonoBehaviour
{

    public KartController kartController; // the kart controller
    public CharacterController characterController; // the character controller

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle") // check the collision tag for "Wall" or "Object"
        {
            kartController.currentThrustForce -= 10f; // slow down the kart

            if (characterController.crashYellTimer == 0) // if the character's crash yell timer is off cooldown
            {
                characterController.crashYellTimer = characterController.crashYellCooldown; // set the character's crash yell timer on cooldown
                kartController.characterClipPlayer.PlayOneShot(0, 5, 1.0f, true); // play the character's crash yell sound
            }
            kartController.kartClipPlayer.PlayRandomAudioClipRange(0, 2, 6, 1.0f); // play the kart's crash sound

        }

    }


}
