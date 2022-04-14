using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class Collectibles
{

    public Dictionary<string, bool[]> registry;

    public Collectibles(Dictionary<string, bool[]> _registry) // class constructor
    {
        registry = _registry; // set the registry based on the input parameter
    }

}
