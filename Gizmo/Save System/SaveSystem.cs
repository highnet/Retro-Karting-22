using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using static FinishLine;
using static GarageManager;
using System;

public static class SaveSystem
{

    public static void SaveGhostRider(GhostRacerRegistry ghostRiderEntries)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to write to file
        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate the path to save the records
        FileStream stream = new FileStream(path, FileMode.Create); // create a new file stream in create mode order to write to file
        formatter.Serialize(stream, ghostRiderEntries); // serialize the records into the filepath using the file stream
        stream.Close(); // close the stream
    }

    public static GhostRacerRegistry LoadGhostRider()
    {
        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to read from file
            FileStream stream = new FileStream(path, FileMode.Open); // create a new file stream in open mode in order to open from file
            GhostRacerRegistry entries = (GhostRacerRegistry)formatter.Deserialize(stream); // deserialize the records
            stream.Close(); // close the stream
            return entries; // return the records
        }
        else
        {
            return null; // else return null
        }
    }
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

    public static GhostRacerRegistry GenerateDefaultGhostRider()
    {
        GhostRaceEntry defaultGhostRaceEntry = new GhostRaceEntry(null,null);
        GhostRaceEntry[][] defaultGhostRaceEntries = new GhostRaceEntry[][]  
        { 
            new GhostRaceEntry[] { defaultGhostRaceEntry, defaultGhostRaceEntry, defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry, defaultGhostRaceEntry  },
            new GhostRaceEntry[] { defaultGhostRaceEntry, defaultGhostRaceEntry, defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry, defaultGhostRaceEntry  },
            new GhostRaceEntry[] { defaultGhostRaceEntry, defaultGhostRaceEntry, defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry , defaultGhostRaceEntry, defaultGhostRaceEntry  }
        };
        return new GhostRacerRegistry(defaultGhostRaceEntries);
    }


    public static void SaveCollectibles(Collectibles collectibles)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to write to file
        string path = Application.persistentDataPath + "/collectibles.sav"; // generate the path to save the records
        FileStream stream = new FileStream(path, FileMode.Create); // create a new file stream in create mode order to write to file
        formatter.Serialize(stream, collectibles); // serialize the records into the filepath using the file stream
        stream.Close(); // close the stream
    }

    public static Collectibles LoadCollectibles()
    {
        string path = Application.persistentDataPath + "/collectibles.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            BinaryFormatter formatter = new BinaryFormatter(); // create a new binary formatter in order to read from file
            FileStream stream = new FileStream(path, FileMode.Open); // create a new file stream in open mode in order to open from file
            Collectibles collectibles = (Collectibles)formatter.Deserialize(stream); // deserialize the records
            stream.Close(); // close the stream
            return collectibles; // return the records
        }
        else
        {
            return null; // else return null
        }
    }

    public static Collectibles GenerateDefaultCollectibles()
    {
        Dictionary<string, bool[]> registry = new Dictionary<string, bool[]>(); // create a new empty registr

        registry.Add(Track.Track0.ToString(), new bool[] { false, false, false, false, false, false, false, false });
        registry.Add(Track.Track1.ToString(), new bool[] { false, false, false, false, false, false, false, false });
        registry.Add(Track.Track2.ToString(), new bool[] { false, false, false, false, false, false, false, false });

        return new Collectibles(registry); // return the registry
    }

    public static Records GenerateDefaultRecords()
    {
        Dictionary<string, List<RecordEntry>> registry = new Dictionary<string, List<RecordEntry>>(); // create a new empty registr
        List<RecordEntry> track0Records = new List<RecordEntry>(); // genereate the track 0 record entry list
        track0Records.Add(new RecordEntry(123, "Heidi", Character.Character0, KartBody.Kart0)); // create the default records
        track0Records.Add(new RecordEntry(127, "Grace", Character.Character1, KartBody.Kart1));
        track0Records.Add(new RecordEntry(130, "Frank", Character.Character1, KartBody.Kart1));
        track0Records.Add(new RecordEntry(134, "Erin", Character.Character0, KartBody.Kart0));
        track0Records.Add(new RecordEntry(135, "Dave", Character.Character1, KartBody.Kart0));
        track0Records.Add(new RecordEntry(136,"Charlie", Character.Character1,KartBody.Kart0));
        track0Records.Add(new RecordEntry(140, "Bob", Character.Character0, KartBody.Kart1));
        track0Records.Add(new RecordEntry(150, "Alice", Character.Character1, KartBody.Kart0));
        track0Records = SortRecordEntries(track0Records); // sort the track entries
        registry.Add(Track.Track0.ToString(), track0Records); // add the records to the registry
        List<RecordEntry> track1records = new List<RecordEntry>(); // genereate the track 1 record entry list
        track1records.Add(new RecordEntry(170, "Heidi", Character.Character0, KartBody.Kart1)); // create the default records
        track1records.Add(new RecordEntry(172, "Grace", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(175, "Frank", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(180, "Erin", Character.Character0, KartBody.Kart0));
        track1records.Add(new RecordEntry(183, "Dave", Character.Character0, KartBody.Kart1));
        track1records.Add(new RecordEntry(194, "Charlie", Character.Character1, KartBody.Kart0));
        track1records.Add(new RecordEntry(200, "Bob", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(210, "Alice", Character.Character1, KartBody.Kart0));
        track1records = SortRecordEntries(track1records); // sort the track entries
        registry.Add(Track.Track1.ToString(), track1records); // add the records to the registry
        List<RecordEntry> track2records = new List<RecordEntry>(); // genereate the track 2 record entry list
        track2records.Add(new RecordEntry(300, "Heidi", Character.Character0, KartBody.Kart1)); // create the default records
        track2records.Add(new RecordEntry(302, "Grace", Character.Character1, KartBody.Kart1));
        track2records.Add(new RecordEntry(305, "Frank", Character.Character1, KartBody.Kart1));
        track2records.Add(new RecordEntry(310, "Erin", Character.Character0, KartBody.Kart0));
        track2records.Add(new RecordEntry(312, "Dave", Character.Character0, KartBody.Kart1));
        track2records.Add(new RecordEntry(314, "Charlie", Character.Character1, KartBody.Kart0));
        track2records.Add(new RecordEntry(317, "Bob", Character.Character1, KartBody.Kart1));
        track2records.Add(new RecordEntry(320, "Alice", Character.Character1, KartBody.Kart0));
        track2records = SortRecordEntries(track2records); // sort the track entries
        registry.Add(Track.Track2.ToString(), track2records); // add the records to the registry
        return new Records(registry); // return the registry
    }

    public static List<RecordEntry> SortRecordEntries(List<RecordEntry> entries)
    {
        return entries.OrderBy(w => w.time).ToList(); // sort the record entries descending ordered by the record time
    }

}
