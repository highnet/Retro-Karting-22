using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static FinishLine;
using System;

public class GhostRacerLoader : MonoBehaviour
{
    public Track track;
    public GhostRiderEntries paths;
    public PathCreator pathCreator;
    public GhostRacer ghostRacer;
    public FinishLine finishLine;
    public bool active;

    public List<Vector3> positions;
    public List<float> timeStamps;

    public int step;
    public bool loaded;


    // Start is called before the first frame update
    void Start()
    {
        paths = SaveSystem.LoadGhostRider();
        pathCreator = GetComponent<PathCreator>();
        ghostRacer = FindObjectOfType<GhostRacer>();
        finishLine = FindObjectOfType<FinishLine>();
        track = finishLine.track;

        if (paths == null) return;
        GhostRaceEntry entry = paths.entries[(int)track];
        if (entry == null) return;
        List<SerializableVector3> pathSVector3 = entry.positions;
        if (pathSVector3 == null) return;


        List<Vector3> pathVector3 = new List<Vector3>();
        for (int i = 0; i < pathSVector3.Count; i++)
        {
            pathVector3.Add(pathSVector3[i]);
        }
        positions = new List<Vector3>(pathVector3);
        timeStamps = new List<float>(entry.timeStamps);
        pathCreator.bezierPath = new BezierPath(pathVector3, false, PathSpace.xyz);

        loaded = true;
     
    }

    public void FixedUpdate()
    {
        if (!loaded || !active) return;

        float distance = 0;
        float time = 0;

        if (step < positions.Count - 1)
        {
            distance = Vector3.Distance(positions[step], positions[step + 1]);
            time = timeStamps[step + 1] - timeStamps[step];
            step++;
        }
        if (distance != 0 && time != 0)
        {
            SetSpeed(distance / time);

        }

    }


    public void SetSpeed(float _speed)
    {
        ghostRacer.pathFollower.speed = _speed;
    }

    public void Activate()
    {
        active = true;
    }
}
