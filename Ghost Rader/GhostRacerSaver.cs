using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinishLine;

public class GhostRacerSaver : MonoBehaviour
{
    public List<SerializableVector3> ghostRacerPositions;
    public List<float> ghostRacerTimestamps;

    public FinishLine finishLine;
    public FinishLine.Track track;
    public int numberOfNodes;
    public RaceController raceController;
    int numberOfGhostRacers = 8;

    private void Start()
    {
        ghostRacerPositions = new List<SerializableVector3>();
        ghostRacerTimestamps = new List<float>();
        finishLine = FindObjectOfType<FinishLine>();
        raceController = FindObjectOfType<RaceController>();
        track = finishLine.track;
    }
    public void Add(Vector3 position, float timestamp)
    {
        ghostRacerPositions.Add(position);
        ghostRacerTimestamps.Add(timestamp);
        numberOfNodes++;
    }

    public void SavePath(Track track)
    {
        Debug.Log("SAVING PATH");
        GhostRaceEntry newEntry = new GhostRaceEntry(ghostRacerPositions, ghostRacerTimestamps);
        GhostRacerRegistry ghostRiderEntries = SaveSystem.LoadGhostRider();

        GhostRaceEntry[][] previousEntries = ghostRiderEntries.entries;

        int indexToPlace = 0;

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


        for (int i = 0; i < numberOfGhostRacers; i++)
        {
            GhostRaceEntry previousEntry = previousEntries[(int)track][i];

            if (previousEntry.timeStamps == null)
            {
                ghostRiderEntries.entries[(int)track][i] = newEntry;
                break;
            }

            if (previousEntry.timeStamps != null)
            {
                float previousEntryElapsedTime = previousEntry.timeStamps[previousEntry.timeStamps.Count - 1];
                if (raceController.totalLapTimer < previousEntryElapsedTime)
                {
                    indexToPlace = i;
                    for (int j = numberOfGhostRacers; j > indexToPlace + 1; j--)
                    {
                        ghostRiderEntries.entries[(int)track][j-1] = ghostRiderEntries.entries[(int)track][j-2];
                    }
                    ghostRiderEntries.entries[(int)track][indexToPlace] = newEntry;
                    break;
                }
            }
        }

        SaveSystem.SaveGhostRider(ghostRiderEntries);

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


    }
}
