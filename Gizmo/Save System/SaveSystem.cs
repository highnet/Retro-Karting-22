using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using static FinishLine;
using static GarageManager;
using System;
using System.Threading.Tasks;

public static class SaveSystem
{

    public static void SaveGhostRider(GhostRacerRegistry ghostRiderEntries)
    {

        GhostRacerRegistrySerializable wrapper = new GhostRacerRegistrySerializable(ghostRiderEntries);

        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate the path to save the records

        string json = JsonUtility.ToJson(wrapper);
        File.WriteAllText(path, json);
    }


    public async static Task SaveGhostRiderAsync(GhostRacerRegistry ghostRiderEntries)
    {
        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate the path to save the records
        string json = "";

        json = await Task.Run(() =>
        {
            GhostRacerRegistrySerializable wrapper = new GhostRacerRegistrySerializable(ghostRiderEntries);
            json = JsonUtility.ToJson(wrapper);
            return json;
        });

        await File.WriteAllTextAsync(path, json);
    }

    public static GhostRacerRegistry LoadGhostRider()
    {
        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate teh path to load ther records
        if (File.Exists(path)) // if the file path exists
        {
           

            string json = File.ReadAllText(path);
            GhostRacerRegistrySerializable wrapper =  JsonUtility.FromJson<GhostRacerRegistrySerializable>(json);

           return wrapper.Unwrap(); 
            

        }
        else
        {
            return null; // else return null
        }
    }

    public async static Task<GhostRacerRegistry> LoadGhostRiderAsync()
    {
        string path = Application.persistentDataPath + "/ghostrider.sav"; // generate teh path to load ther records
        if (File.Exists(path)) // if the file path exists
        {
            GhostRacerRegistry registry = null;



            string json = await File.ReadAllTextAsync(path);
            registry  = await Task.Run(() =>
            {
                GhostRacerRegistrySerializable wrapper = JsonUtility.FromJson<GhostRacerRegistrySerializable>(json);
                registry = wrapper.Unwrap();
                return registry;
            });


              
          if(registry != null) 
            {
                return registry;
            } else
            {
                return await Task.FromResult<GhostRacerRegistry>(null); // else return null
            }
           
        }
        else
        {
            return await Task.FromResult<GhostRacerRegistry>(null); // else return null
        }
    }

    public static void SaveRecords(Records records)
    {

        RecordsSerializable wrapper = new RecordsSerializable(records);

        string path = Application.persistentDataPath + "/records.sav"; // generate the path to save the records

        string json = JsonUtility.ToJson(wrapper);

        File.WriteAllText(path, json);

    }

    public async static Task SaveRecordsAsync(Records records)
    {

       

        string path = Application.persistentDataPath + "/records.sav"; // generate the path to save the records
        string json = "";

        json =  await Task.Run(()=> {
            RecordsSerializable wrapper = new RecordsSerializable(records);
            json = JsonUtility.ToJson(wrapper);
            return json;
        });


        await File.WriteAllTextAsync(path, json);

    }



    public static Records LoadRecords()
    {
        string path = Application.persistentDataPath + "/records.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            string json = File.ReadAllText(path);
            RecordsSerializable wrapper = JsonUtility.FromJson<RecordsSerializable>(json);

            return wrapper.Unwrap();
        } else
        {
            return null; // else return null
        }
    }

    public async static Task<Records> LoadRecordsAsync()
    {
        string path = Application.persistentDataPath + "/records.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            Records records = null;

            string json = await File.ReadAllTextAsync(path);
            records = await Task.Run(()=> 
            { 
                RecordsSerializable wrapper = JsonUtility.FromJson<RecordsSerializable>(json);
                records = wrapper.Unwrap();
                return records;
            });

   
            RecordsSerializable wrapper = JsonUtility.FromJson<RecordsSerializable>(json);

            if (records != null) {
                return records;
            } else
            {
                return await Task.FromResult<Records>(null); // else return null
            }
            
        }
        else
        {
            return await Task.FromResult<Records>(null); // else return null
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

        CollectiblesSerializable wrapper = new CollectiblesSerializable(collectibles);

        string path = Application.persistentDataPath + "/collectibles.sav"; // generate the path to save the records

        string json = JsonUtility.ToJson(wrapper);

        File.WriteAllText(path, json);
    }



    public static Collectibles LoadCollectibles()
    {
        string path = Application.persistentDataPath + "/collectibles.sav"; // generate teh path to load ther ecords
        if (File.Exists(path)) // if the file path exists
        {
            string json = File.ReadAllText(path);
            CollectiblesSerializable wrapper = JsonUtility.FromJson<CollectiblesSerializable>(json);
            return wrapper.Unwrap(); // return the records
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
        track0Records.Add(new RecordEntry(123, "Maria", Character.Character0, KartBody.Kart0)); // create the default records
        track0Records.Add(new RecordEntry(127, "Issur", Character.Character1, KartBody.Kart1));
        track0Records.Add(new RecordEntry(130, "Orla", Character.Character1, KartBody.Kart1));
        track0Records.Add(new RecordEntry(134, "Malaika", Character.Character0, KartBody.Kart0));
        track0Records.Add(new RecordEntry(135, "Tamya", Character.Character1, KartBody.Kart0));
        track0Records.Add(new RecordEntry(136,"Draha", Character.Character1,KartBody.Kart0));
        track0Records.Add(new RecordEntry(140, "Preethi", Character.Character0, KartBody.Kart1));
        track0Records.Add(new RecordEntry(150, "Vanna", Character.Character1, KartBody.Kart0));
        track0Records = SortRecordEntries(track0Records); // sort the track entries
        registry.Add(Track.Track0.ToString(), track0Records); // add the records to the registry
        List<RecordEntry> track1records = new List<RecordEntry>(); // genereate the track 1 record entry list
        track1records.Add(new RecordEntry(170, "Diego", Character.Character0, KartBody.Kart1)); // create the default records
        track1records.Add(new RecordEntry(172, "Dimitri", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(175, "Afina", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(180, "Themis", Character.Character0, KartBody.Kart0));
        track1records.Add(new RecordEntry(183, "Donna", Character.Character0, KartBody.Kart1));
        track1records.Add(new RecordEntry(194, "Milo", Character.Character1, KartBody.Kart0));
        track1records.Add(new RecordEntry(200, "Bob", Character.Character1, KartBody.Kart1));
        track1records.Add(new RecordEntry(210, "Alice", Character.Character1, KartBody.Kart0));
        track1records = SortRecordEntries(track1records); // sort the track entries
        registry.Add(Track.Track1.ToString(), track1records); // add the records to the registry
        List<RecordEntry> track2records = new List<RecordEntry>(); // genereate the track 2 record entry list
        track2records.Add(new RecordEntry(240, "Manuel", Character.Character1, KartBody.Kart0)); // create the default records
        track2records.Add(new RecordEntry(242, "Peleg", Character.Character0, KartBody.Kart1));
        track2records.Add(new RecordEntry(245, "Jacobus", Character.Character0, KartBody.Kart0));
        track2records.Add(new RecordEntry(250, "Oleksandr", Character.Character0, KartBody.Kart0));
        track2records.Add(new RecordEntry(252, "Vienne", Character.Character0, KartBody.Kart1));
        track2records.Add(new RecordEntry(254, "Lamnot", Character.Character0, KartBody.Kart0));
        track2records.Add(new RecordEntry(257, "Irene", Character.Character0, KartBody.Kart0));
        track2records.Add(new RecordEntry(260, "Ronen", Character.Character1, KartBody.Kart0));
        track2records = SortRecordEntries(track2records); // sort the track entries
        registry.Add(Track.Track2.ToString(), track2records); // add the records to the registry
        return new Records(registry); // return the registry
    }

    public static List<RecordEntry> SortRecordEntries(List<RecordEntry> entries)
    {
        return entries.OrderBy(w => w.time).ToList(); // sort the record entries descending ordered by the record time
    }

}
