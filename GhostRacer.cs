using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
public class GhostRacer : MonoBehaviour
{
    public PathFollower pathFollower;
    // Start is called before the first frame update
    void Start()
    {
        pathFollower = GetComponent<PathFollower>();
    }

    private void LateUpdate()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, 0f);
    }
}
