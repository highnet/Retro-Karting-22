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

    private void Start()
    {
        ghostRacerPositions = new List<SerializableVector3>();
        ghostRacerTimestamps = new List<float>();
        finishLine = FindObjectOfType<FinishLine>();
        track = finishLine.track;
    }
    public void Add(Vector3 position,float timestamp)
    {
        ghostRacerPositions.Add(position);
        ghostRacerTimestamps.Add(timestamp);
        numberOfNodes++;
    }

    public void SavePath(Track track, float elapsedTime)
    {
        GhostRaceEntry newEntry = new GhostRaceEntry(ghostRacerPositions, ghostRacerTimestamps);
        GhostRiderEntries ghostRiderEntries = SaveSystem.LoadGhostRider();

        GhostRaceEntry[] previousEntries = ghostRiderEntries.entries;
        GhostRaceEntry previousEntry = previousEntries[(int)track];

        if (previousEntry.timeStamps != null)
        {
            float previousEntryElapsedTime = previousEntry.timeStamps[previousEntry.timeStamps.Count - 1];
            if (previousEntryElapsedTime < elapsedTime) return;

        }


        ghostRiderEntries.entries[(int)track] = newEntry;
        SaveSystem.SaveGhostRider(ghostRiderEntries);
    }
}
