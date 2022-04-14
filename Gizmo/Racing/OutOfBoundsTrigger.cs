using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    KartController kartController;

    private void Start()
    {
        kartController = FindObjectOfType<KartController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            kartController.Respawn();
        }
    }
}
