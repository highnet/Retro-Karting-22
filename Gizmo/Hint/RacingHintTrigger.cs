using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingHintTrigger : MonoBehaviour
{

    private UIRacingHintsWindow racingHintsWindow; // the racing hints window
    public int racingHintID; // the local id of the hint to activate

    private void Start()
    {
        racingHintsWindow = FindObjectOfType<UIRacingHintsWindow>(); // store a reference to the racing hints window
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") // check the collider tag for "Player"
        {
            if (!racingHintsWindow.active) // check that the hints window is currently inactive
            {
                racingHintsWindow.TriggerRacingHint(racingHintID); // trigger a speciif hint from the hints window

            }
        }
    }
}
