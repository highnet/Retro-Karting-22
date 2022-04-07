using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    private UIHintsWindow hintsWindow; // the ui hints window
    private void Start()
    {
        hintsWindow = FindObjectOfType<UIHintsWindow>(); // store a local reference to the ui hints window
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") // check the collider tag for "Player"
        {
            if (!hintsWindow.active) // check that the hints window is currently inactive
            {
                hintsWindow.TriggerRandomHint(); // trigger a random hint from the hints window

            }
        }

    }
}
