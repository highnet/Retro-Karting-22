using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrapTrigger : MonoBehaviour
{


    public GameObject trapObject;                          
    public GameObject trapSpawnPoint;
    public bool wasTriggered;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !wasTriggered)            // check if other is player
        {

            trapObject.transform.position = trapSpawnPoint.transform.position;     // copy trap spawn point rotation and position
            trapObject.transform.rotation = trapSpawnPoint.transform.rotation;

             if (!trapObject.activeSelf)
            {
                trapObject.SetActive(true);           // if necessary enable trap object
            }

            wasTriggered = true;

        }
    }
}
