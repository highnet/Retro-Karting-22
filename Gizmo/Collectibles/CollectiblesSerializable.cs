using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesSerializable
{

    public List<string> keys;
    public List<BoolArrayWrapper> values;


    public CollectiblesSerializable(Collectibles collectibles)
    {

        keys = new List<string>();
        values = new List<BoolArrayWrapper>();

        foreach (var keyValuePair in collectibles.registry)
        {

            keys.Add(keyValuePair.Key);
            BoolArrayWrapper arrayWrapper = new BoolArrayWrapper();
            arrayWrapper.wrappedValue = keyValuePair.Value;
            values.Add(arrayWrapper);
        }

    }

    public Collectibles Unwrap()
    {


        Dictionary<string, bool[]> dictionary = new Dictionary<string, bool[]>();


        for (int i = 0; i < System.Math.Min(keys.Count, values.Count); i++)
        {
            dictionary.Add(keys[i], values[i].wrappedValue);

        }

        return new Collectibles(dictionary);

    }

    [System.Serializable] // tag this class as serializable as it will be written to local file storage
    public class BoolArrayWrapper
    {

        public bool[] wrappedValue;

    }
}
