using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private void FixedUpdate()
    {
        this.transform.RotateAround(this.transform.position, Vector3.Normalize(Vector3.up + Vector3.right), 5f * Time.deltaTime); // rotate the planet around its own center of mass
    }
}
