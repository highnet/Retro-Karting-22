using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool active; // the activity status of the checkpoint

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // check the collision tag for "Player"
        {
            active = true; // if collider with a player, set to active
        }

    }
}
