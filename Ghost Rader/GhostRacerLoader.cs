using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static FinishLine;
using System;

public class GhostRacerLoader : MonoBehaviour
{
    public Track track;
    public GhostRacerRegistry registry;
    public PathCreator[] pathCreators;
    public GhostRacer[] ghostRacers;
    public FinishLine finishLine;
    public bool active;
    public List<Vector3>[] positions;
    public List<float>[] timeStamps;

    public int step;
    public bool[] loaded;
    int numberOfGhostRacers = 8;



    // Start is called before the first frame update
    void Start()
    {
        registry = SaveSystem.LoadGhostRider();
        pathCreators = GetComponents<PathCreator>();
        ghostRacers = FindObjectsOfType<GhostRacer>();
        finishLine = FindObjectOfType<FinishLine>();
        track = finishLine.track;

        positions = new List<Vector3>[numberOfGhostRacers];
        timeStamps = new List<float>[numberOfGhostRacers];
        loaded = new bool[numberOfGhostRacers];

        if (registry == null) return;

        for (int i = 0; i < numberOfGhostRacers; i++)
        {
            GhostRaceEntry entry = registry.entries[(int)track][i];
            if (entry != null)
            {
                List<SerializableVector3> pathSVector3 = entry.positions;
                if (pathSVector3 != null)
                {
                    List<Vector3> pathVector3 = new List<Vector3>();
                    for (int j = 0; j < pathSVector3.Count; j++)
                    {
                        pathVector3.Add(pathSVector3[j]);
                    }
                    positions[i] = new List<Vector3>(pathVector3);
                    timeStamps[i] = new List<float>(entry.timeStamps);
                    pathCreators[i].bezierPath = new BezierPath(pathVector3, false, PathSpace.xyz);
                    loaded[i] = true;
                }
            }
        }

    }

    public void FixedUpdate()
    {
        if (!active) return;

        float distance = 0;
        float time = 0;

 
        for(int i = 0; i < numberOfGhostRacers; i++)
        {
            if (loaded[i])
            {
                if (step < positions[i].Count - 1)
                {
                    distance = Vector3.Distance(positions[i][step], positions[i][step + 1]);
                    time = timeStamps[i][step + 1] - timeStamps[i][step];
                    step++;
                }
                if (distance != 0 && time != 0)
                {
                    SetSpeed(i, distance / time);
                }
            }

        }
    }


    public void SetSpeed(int ghostRacerId, float _speed)
    {
        ghostRacers[ghostRacerId].pathFollower.speed = _speed;
    }

    public void Activate()
    {
        active = true;
    }
}
