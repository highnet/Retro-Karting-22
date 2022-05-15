using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class Records 
{
    public Dictionary<string, List<RecordEntry>> registry; // the records registry for reach track [KEY = track] [VALUES = list of record entries sorted descending] 

    public Records(Dictionary<string, List<RecordEntry>>  _registry) // class constructor
    {  
        registry = _registry; // set the registry based on the input parameter
    }

}
