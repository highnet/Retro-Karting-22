using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsCollider : MonoBehaviour
{
    KartController kartController;

    private void Start()
    {
        kartController = FindObjectOfType<KartController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            kartController.Respawn();
        }
    }

}
