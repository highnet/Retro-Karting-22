using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static FinishLine;

public class GhostRacerSaver : MonoBehaviour
{
    public FinishLine finishLine; // the finish line
    public FinishLine.Track track; // the track
    public RaceController raceController; // the race controller
    int numberOfGhostRacers = 8; // the number of ghost racers on registry

    public List<SerializableVector3> ghostRacerPositions; // the list of serializable vector 3 positions that will be saved as a new possible entry on the registry
    public List<float> ghostRacerTimestamps; // the list of float timestamps that will be saved as a new possible entry on the registry
    public int numberOfNodes; // the number of nodes on the new path

    private void Start()
    {
        ghostRacerPositions = new List<SerializableVector3>(); // create a new empty serializable vector 3 list
        ghostRacerTimestamps = new List<float>(); // create a new float list
        finishLine = FindObjectOfType<FinishLine>(); // store a local reference to the finish line
        raceController = FindObjectOfType<RaceController>(); // store a local reference to the race controller
        track = finishLine.track; // the track
    }
    public void Add(Vector3 position, float timestamp)
    {
        ghostRacerPositions.Add(position); // add the ghost racer position
        ghostRacerTimestamps.Add(timestamp); // add the ghost racer time stamp
        numberOfNodes++; // increase the number of nodes counter
    }

    public IEnumerator SavePathAfterSeconds(Track track, float seconds)
    {
        yield return new WaitForSeconds(seconds); // wait for a certain amount of seconds
        SavePath(track);

    }

    public void SavePath(Track track)
    {
        Debug.Log("SAVING PATH");
        GhostRaceEntry newEntry = new GhostRaceEntry(ghostRacerPositions, ghostRacerTimestamps); // create a new entry with the saved positions and timestamps
        GhostRacerRegistry ghostRiderEntries = SaveSystem.LoadGhostRider(); // load the registry

        GhostRaceEntry[][] previousEntries = ghostRiderEntries.entries; // load the previous entries

        int indexToPlace = 0; // the index at which the new entry will be placed

        /*
        Debug.Log("=================");
        for (int i = 0; i < numberOfGhostRacers; i++)
        {

            if (ghostRiderEntries.entries[(int)track][i].timeStamps == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log(ghostRiderEntries.entries[(int)track][i].timeStamps[ghostRiderEntries.entries[(int)track][i].timeStamps.Count - 1]);
            }
        }
        Debug.Log("=================");
        */


        for (int i = 0; i < numberOfGhostRacers; i++) // for each number of ghost racers or until we write a new ghost racer entry
        {
            GhostRaceEntry previousEntry = previousEntries[(int)track][i]; // fetch the ghost racer entry

            if (previousEntry.timeStamps == null) // check if the entry is null
            {
                ghostRiderEntries.entries[(int)track][i] = newEntry; // overwrite the null entry right away
                break; // break the loop
            }

            if (previousEntry.timeStamps != null) // check if the entry is non null
            {
                float previousEntryElapsedTime = previousEntry.timeStamps[previousEntry.timeStamps.Count - 1]; // fetch the elapsted time
                if (raceController.totalLapTimer < previousEntryElapsedTime) // if our time is smaller than the elapsed time
                {
                    indexToPlace = i; // set the index of the entry to replace to i 
                    for (int j = numberOfGhostRacers; j > indexToPlace + 1; j--) // for all other entries to the right of the one which we will replace
                    {
                        ghostRiderEntries.entries[(int)track][j-1] = ghostRiderEntries.entries[(int)track][j-2]; // shift the entries by one
                    }
                    ghostRiderEntries.entries[(int)track][indexToPlace] = newEntry; // overwrite the slower entry
                    break; // break the loop
                }
            }
        }

        SaveSystem.SaveGhostRider(ghostRiderEntries);

        /*
        Debug.Log("=================");
        for (int i = 0; i < numberOfGhostRacers; i++)
        {

            if (ghostRiderEntries.entries[(int)track][i].timeStamps == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log(ghostRiderEntries.entries[(int)track][i].timeStamps[ghostRiderEntries.entries[(int)track][i].timeStamps.Count - 1]);
            }
        }
        Debug.Log("=================");
        */


    }

    public async Task SavePathAsync(Track track)
    {
        Debug.Log("SAVING PATH ASYNC");

        GhostRaceEntry newEntry = new GhostRaceEntry(ghostRacerPositions, ghostRacerTimestamps); // create a new entry with the saved positions and timestamps
        GhostRacerRegistry ghostRiderEntries =  await SaveSystem.LoadGhostRiderAsync(); // load the registry

        GhostRaceEntry[][] previousEntries = ghostRiderEntries.entries; // load the previous entries

        int indexToPlace = 0; // the index at which the new entry will be placed

        /*
        Debug.Log("=================");
        for (int i = 0; i < numberOfGhostRacers; i++)
        {

            if (ghostRiderEntries.entries[(int)track][i].timeStamps == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log(ghostRiderEntries.entries[(int)track][i].timeStamps[ghostRiderEntries.entries[(int)track][i].timeStamps.Count - 1]);
            }
        }
        Debug.Log("=================");
        */


        for (int i = 0; i < numberOfGhostRacers; i++) // for each number of ghost racers or until we write a new ghost racer entry
        {
            GhostRaceEntry previousEntry = previousEntries[(int)track][i]; // fetch the ghost racer entry

            if (previousEntry.timeStamps == null) // check if the entry is null
            {
                ghostRiderEntries.entries[(int)track][i] = newEntry; // overwrite the null entry right away
                break; // break the loop
            }

            if (previousEntry.timeStamps != null) // check if the entry is non null
            {
                float previousEntryElapsedTime = previousEntry.timeStamps[previousEntry.timeStamps.Count - 1]; // fetch the elapsted time
                if (raceController.totalLapTimer < previousEntryElapsedTime) // if our time is smaller than the elapsed time
                {
                    indexToPlace = i; // set the index of the entry to replace to i 
                    for (int j = numberOfGhostRacers; j > indexToPlace + 1; j--) // for all other entries to the right of the one which we will replace
                    {
                        ghostRiderEntries.entries[(int)track][j - 1] = ghostRiderEntries.entries[(int)track][j - 2]; // shift the entries by one
                    }
                    ghostRiderEntries.entries[(int)track][indexToPlace] = newEntry; // overwrite the slower entry
                    break; // break the loop
                }
            }
        }

         await SaveSystem.SaveGhostRiderAsync(ghostRiderEntries);
        Debug.Log("ASYNC SAVE FINISHED");
        /*
        Debug.Log("=================");
        for (int i = 0; i < numberOfGhostRacers; i++)
        {

            if (ghostRiderEntries.entries[(int)track][i].timeStamps == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log(ghostRiderEntries.entries[(int)track][i].timeStamps[ghostRiderEntries.entries[(int)track][i].timeStamps.Count - 1]);
            }
        }
        Debug.Log("=================");
        */


    }
}
