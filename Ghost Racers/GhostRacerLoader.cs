using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static FinishLine;
using System;

public class GhostRacerLoader : MonoBehaviour
{
    public FinishLine finishLine; // the finish line
    public Track track; // the current track
    int numberOfGhostRacers = 8; // the number of ghost racers to be loaded
    public GhostRacerRegistry registry; // the ghost racer registry
    public PathCreator[] pathCreators; // the array of path creators
    public GhostRacer[] ghostRacers; // the array of ghost racers
    public bool active; // the activity status of the ghost racer loader
    public List<Vector3>[] positions; // the array of list of positions
    public List<float>[] timeStamps; // the array of list of timestamps
    public bool[] loaded; // the array of statuses of wether each ghost racer has properly been loaded from file
    public int step; // the simulation step increment

    // Start is called before the first frame update
    void Start()
    {
        registry = SaveSystem.LoadGhostRider(); // load the ghost rider registry from the file save system
        pathCreators = GetComponents<PathCreator>(); // store a local reference to the path creators
        finishLine = FindObjectOfType<FinishLine>(); // store a local reference to the finish line
        track = finishLine.track; // store a local reference to the track

        positions = new List<Vector3>[numberOfGhostRacers]; // create a vector3 new list of size numberOfGhostRacers
        timeStamps = new List<float>[numberOfGhostRacers]; // create a new float list of size numberOfGhostRacers
        loaded = new bool[numberOfGhostRacers]; // create a new boolean array of size numberOfGhostRacers

        if (registry == null) return; // check if registry is null, do nothing if the registry is null

        for (int i = 0; i < numberOfGhostRacers; i++) // for each ghost racer index
        {
            GhostRaceEntry entry = registry.entries[(int)track][i]; // fetch the entry from the 2d array at index [(int)track],[i]
            if (entry != null) // check if the entry is not null
            {
                List<SerializableVector3> pathSVector3 = entry.positions; // fetch the list of serializable vector 3 positions from the entry
                if (pathSVector3 != null) // check if the list is not null
                {
                    List<Vector3> pathVector3 = new List<Vector3>(); // create a new list of vector 3 positions (non serializible)
                    for (int j = 0; j < pathSVector3.Count; j++) // for each element in the serializable vector 3 list
                    {
                        pathVector3.Add(pathSVector3[j]);  // copy it to the vector3 list

                    }
                    positions[i] = new List<Vector3>(pathVector3); // set the list of vector 3 positions at the ith index
                    timeStamps[i] = new List<float>(entry.timeStamps); // set the list of float timestamps at the ith index
                    pathCreators[i].bezierPath = new BezierPath(pathVector3, false, PathSpace.xyz); // generate and set new bezier path
                    loaded[i] = true; // flag the ith index as loaded
                }
            }
        }

    }

    public void FixedUpdate()
    {
        if (!active) return; // check if the ghost race loader is flagged as active, return otherwise
        UpdateLoadedGhostRacerSpeeds();
    }

    public void UpdateLoadedGhostRacerSpeeds()
    {
        float distance = 0; // variable used to calculate the distance from point to point
        float time = 0;  // variable used to calculate the time it should take to travel from point to point

        for (int i = 0; i < numberOfGhostRacers; i++) // for each ghost racer
        {
            if (loaded[i]) // check if the ghost racer is loaded
            {
                if (step < positions[i].Count - 1) // check if we are not at the last step of the path
                {
                    distance = Vector3.Distance(positions[i][step], positions[i][step + 1]); // calculate the distance from the point at the current step to the point at the next step
                    time = timeStamps[i][step + 1] - timeStamps[i][step]; // calculate the elapsed time from the point at the current step and the point to the next step
                }
                if (distance != 0 && time != 0) // check that time and distance are non zero
                {
                    SetSpeed(i, distance / time); // modulate the speed to satisfy the linear travel of a given distance at a given time
                }
            }
        }
        step++; // increment the step
    }


    public void SetSpeed(int ghostRacerId, float _speed)
    {
        ghostRacers[ghostRacerId].pathFollower.speed = _speed; // set the speed of the ghost racer
    }

    public void Activate()
    {
        active = true; // activate the ghost racer loader
    }
}
