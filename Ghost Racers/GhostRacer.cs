using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
public class GhostRacer : MonoBehaviour
{
    public PathFollower pathFollower; // the path follower

    void Start()
    {
        pathFollower = GetComponent<PathFollower>(); // store a local reference to the path follower
    }

    private void LateUpdate()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0f); // set the z rotation to 0 so it lies flat on the plane
    }
}
