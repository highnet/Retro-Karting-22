using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class RecordsSerializable
{

    public List<string> keys;
    public List<RecordListWrapper> values;

    public RecordsSerializable(Records records)
    {

        keys = new List<string>();
        values = new List<RecordListWrapper>();

        foreach (var keyValuePair in records.registry)
        {

            keys.Add(keyValuePair.Key);
            RecordListWrapper listWrapper = new RecordListWrapper();
            listWrapper.wrappedValue = keyValuePair.Value;
            values.Add(listWrapper);
        }

    }


    public Records Unwrap()
    {


        Dictionary<string, List<RecordEntry>> dictionary = new Dictionary<string, List<RecordEntry>>();


        for (int i = 0; i < System.Math.Min(keys.Count, values.Count); i++)
        {
            dictionary.Add(keys[i], values[i].wrappedValue);

        }

        return new Records(dictionary);

    }


    [System.Serializable] // tag this class as serializable as it will be written to local file storage
    public class RecordListWrapper{

       public List<RecordEntry> wrappedValue;

        public RecordListWrapper()
        {
            wrappedValue = new List<RecordEntry>();
        }
    
    }

}
