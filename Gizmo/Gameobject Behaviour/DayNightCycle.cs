using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    private void FixedUpdate()
    {
        this.transform.RotateAround(this.transform.position, Vector3.Normalize(Vector3.up + Vector3.right), 5f * Time.deltaTime); // rotate the sun around its own center of mass to simulate a day and night cycle
    }
}
