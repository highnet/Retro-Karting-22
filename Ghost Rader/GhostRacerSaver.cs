using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinishLine;

public class GhostRacerSaver : MonoBehaviour
{
    public List<SerializableVector3> ghostRiderPath;
    public FinishLine finishLine;
    public FinishLine.Track track;

    private void Start()
    {
        ghostRiderPath = new List<SerializableVector3>();
        finishLine = FindObjectOfType<FinishLine>();
        track = finishLine.track;
    }
    public void Add(Vector3 position)
    {
        ghostRiderPath.Add(position);
    }

    public void SavePath(Track track, float elapsedTime)
    {
        GhostRaceEntry newEntry = new GhostRaceEntry(ghostRiderPath, elapsedTime);
        GhostRiderEntries ghostRiderEntries = SaveSystem.LoadGhostRider();
        if (ghostRiderEntries.entries[(int)track].elapsedTime != 0 && ghostRiderEntries.entries[(int)track].elapsedTime < elapsedTime) return;
        ghostRiderEntries.entries[(int)track] = newEntry;
        SaveSystem.SaveGhostRider(ghostRiderEntries);
    }
}
