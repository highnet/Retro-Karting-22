using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPlatform : MonoBehaviour
{

    private KartController kartController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // check the collision tag for "Player"
        {
            if (kartController == null) // if kartcontroller exists
            {
                kartController = FindObjectOfType<KartController>(); // find the kart controller script
            }
            kartController.powerup.DoMegaSpeedUp(); // apply the mega speedup to the kart
        }
    }
}
