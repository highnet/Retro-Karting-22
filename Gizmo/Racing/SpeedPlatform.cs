using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedPlatformType {PowerUp, VelocityMod}

public class SpeedPlatform : MonoBehaviour
{

    private KartController kartController;
    public SpeedPlatformType platformType;
    public float speedChange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // check the collision tag for "Player"
        {
            if (kartController == null) // if kartcontroller exists
            {
                kartController = GameObject.FindGameObjectWithTag("Controller").GetComponent<KartController>(); // find the kart controller script
            }

            switch (platformType)
            {
                case SpeedPlatformType.VelocityMod:
                    kartController.powerup.DoDirectedSpeedChange(speedChange, transform.up);
                    break;
                case SpeedPlatformType.PowerUp:
                    kartController.powerup.DoSpeedUp(); // apply the mega speedup to the kart
                    break;
                default:
                    kartController.powerup.DoSpeedUp(); // apply the mega speedup to the kart
                    break;
            }
           

          
          //kartController.SpeedBoost(speedUpStrength, Color.green, 1.0f, 1.0f, 1.0f, 20f, true, true, ForceMode.Acceleration); // apply the speedboost to the kart
        }
    }
}
