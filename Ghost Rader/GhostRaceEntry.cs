using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class GhostRaceEntry
{
    public List<SerializableVector3> positions;
    public List<float> timeStamps;

    public GhostRaceEntry(List<SerializableVector3> _positions , List<float> _timeStamps) // class constructor
    {
        positions = _positions; // set the registry based on the input parameter
        timeStamps = _timeStamps;
    }


}
