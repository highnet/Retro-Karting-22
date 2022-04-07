using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using static FinishLine;
using static GarageManager;

public static class SaveSystem
{
    public static void SaveRecords(Records records)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to write to file
        string path = Application.persistentDataPath + "/records.sav"; // generate the path to save the records
        FileStream stream = new FileStream(path, FileMode.Create); // create a new file stream in create mode order to write to file
        formatter.Serialize(stream, records); // serialize the records into the filepath using the file stream
        stream.Close(); // close the stream
    }

    public static Records LoadRecords()
    {
        string path = Application.persistentDataPath + "/records.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to read from file
            FileStream stream = new FileStream(path, FileMode.Open); // create a new file stream in open mode in order to open from file
            Records records = (Records) formatter.Deserialize(stream); // deserialize the records
            stream.Close(); // close the stream
            return records; // return the records
        } else
        {
            return null; // else return null
        }
    }

    public static Records GenerateDefaultRecords()
    {
        Dictionary<string, List<RecordEntry>> registry = new Dictionary<string, List<RecordEntry>>(); // create a new empty registr
        List<RecordEntry> track0Records = new List<RecordEntry>(); // genereate the track 0 record entry list
        track0Records.Add(new RecordEntry(300, "Heidi", Character.Character0, KartBody.Kart0)); // create the default records
        track0Records.Add(new RecordEntry(290, "Grace", Character.Character0, KartBody.Kart1));
        track0Records.Add(new RecordEntry(280, "Frank", Character.Character1, KartBody.Kart0));
        track0Records.Add(new RecordEntry(270, "Erin", Character.Character1, KartBody.Kart0));
        track0Records.Add(new RecordEntry(260, "Dave", Character.Character1, KartBody.Kart1));
        track0Records.Add(new RecordEntry(250,"Charlie", Character.Character0,KartBody.Kart0));
        track0Records.Add(new RecordEntry(240, "Bob", Character.Character0, KartBody.Kart1));
        track0Records.Add(new RecordEntry(230, "Alice", Character.Character0, KartBody.Kart1));
        track0Records = SortRecordEntries(track0Records); // sort the track entries
        registry.Add(Track.Track0.ToString(), track0Records); // add the records to the registry
        List<RecordEntry> track1records = new List<RecordEntry>(); // genereate the track 1 record entry list
        track1records.Add(new RecordEntry(300, "Heidi", Character.Character0, KartBody.Kart0)); // create the default records
        track1records.Add(new RecordEntry(290, "Grace", Character.Character0, KartBody.Kart1));
        track1records.Add(new RecordEntry(280, "Frank", Character.Character1, KartBody.Kart0));
        track1records.Add(new RecordEntry(270, "Erin", Character.Character1, KartBody.Kart0));
        track1records.Add(new RecordEntry(260, "Dave", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(250, "Charlie", Character.Character1, KartBody.Kart0));
        track1records.Add(new RecordEntry(240, "Bob", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(230, "Alice", Character.Character0, KartBody.Kart1));
        track1records = SortRecordEntries(track1records); // sort the track entries
        registry.Add(Track.Track1.ToString(), track1records); // add the records to the registry
        return new Records(registry); // return the registry
    }

    public static List<RecordEntry> SortRecordEntries(List<RecordEntry> entries)
    {
        return entries.OrderBy(w => w.time).ToList(); // sort the record entries descending ordered by the record time
    }

}
