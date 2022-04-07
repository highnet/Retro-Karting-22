using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GarageManager;

[System.Serializable]
public class RecordEntry
{
    public float time; // the total race time of the record
    public string author; // the author's name of the record
    public Character character; // the character the player used to create the record
    public KartBody kart; // the kart the player used to create the record

    public RecordEntry(float _time, string _author, Character _character, KartBody _kart) // class constructor
    {
        time = _time; // set the time
        author = _author; // set the author
        character = _character; // set the character
        kart = _kart; // set the kart
    }
}
