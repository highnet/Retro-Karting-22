using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsCollider : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Controller").GetComponent<KartController>().Respawn();
        }
    }

}
