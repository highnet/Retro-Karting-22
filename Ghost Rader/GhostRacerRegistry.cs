using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class GhostRacerRegistry
{
    public GhostRaceEntry[][] entries;
    public GhostRacerRegistry(GhostRaceEntry[][] _entries) // class constructor
    {
        entries = _entries; // set the registry based on the input parameter
    }
}
