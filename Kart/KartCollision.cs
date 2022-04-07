using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartCollision : MonoBehaviour
{

    public KartController kartController;
    public CharacterController characterController;


    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Obstacle")
        {
            kartController.currentThrustForce -= 10f;

            if (characterController.crashYellTimer == 0)
            {
                characterController.crashYellTimer = characterController.crashYellCooldown;
                kartController.characterClipPlayer.PlayOneShot(0, 5, 1.0f, true);
            }
            kartController.kartClipPlayer.PlayRandomAudioClipRange(0, 2, 6, 1.0f);

        }

    }


}
