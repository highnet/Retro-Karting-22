using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class GhostRaceEntry
{
    public List<SerializableVector3> nodes;
    public float elapsedTime;

    public GhostRaceEntry(List<SerializableVector3> _nodes , float _elapsedTime) // class constructor
    {
        nodes = _nodes; // set the registry based on the input parameter
        elapsedTime = _elapsedTime;
    }


}
