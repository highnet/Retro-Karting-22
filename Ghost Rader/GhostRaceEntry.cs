using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class GhostRaceEntry
{
    public List<SerializableVector3> positions; // the list of positions
    public List<float> timeStamps; // the list of timestamps

    public GhostRaceEntry(List<SerializableVector3> _positions , List<float> _timeStamps) // class constructor
    {
        positions = _positions; // set the positions
        timeStamps = _timeStamps; // set the timestamps
    }
}
