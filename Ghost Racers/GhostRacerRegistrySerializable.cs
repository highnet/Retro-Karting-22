using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable] // tag this class as serializable as it will be written to local file storage
public class GhostRacerRegistrySerializable
{
    public GhostRaceEntry[] entries; // the array of entries as vector 2, the first coordinate defines the track id and the second coordinate the ghost racer id
    public int length;
    public int subLenght; 

    public GhostRacerRegistrySerializable(GhostRacerRegistry registry) // class constructor
    {
        length = registry.entries.Length;
        subLenght = 8;
        int totalSubLength = 0;


        foreach (GhostRaceEntry[] arr in registry.entries)
        {

            totalSubLength += subLenght;

        }


        int size = totalSubLength;
        entries = new GhostRaceEntry[size];

        int index = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < subLenght; j++)
            {

                entries[index++] = registry.entries[i][j];

            }

        }


    }

    public GhostRacerRegistry Unwrap()
    {
        GhostRaceEntry[][] entryArr = new GhostRaceEntry[length][];


        for (int i = 0; i < length; i++)
        {
            entryArr[i] = new GhostRaceEntry[subLenght];


            for (int j = 0; j < subLenght; j++)
            {
                if (entries[( i * subLenght) + j ].positions.Count == 0)
                {
                    entryArr[i][j] = new GhostRaceEntry(null, null);
                } else{

                    entryArr[i][j] =  entries[(i * subLenght) + j];
                }
      
            }
        }


          return new GhostRacerRegistry(entryArr);
    }
}
