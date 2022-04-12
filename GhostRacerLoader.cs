using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using static FinishLine;

public class GhostRacerLoader : MonoBehaviour
{
    public Track track;
    public GhostRiderEntries paths;
    public PathCreator pathCreator;
    public GhostRacer ghostRacer;
    public float speed;
    public float pathLength;
    public FinishLine finishLine;


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
        List<SerializableVector3> path = entry.nodes;
        if (path == null) return;
        int numberOfNodes = path.Count;
        float elapsedTime = entry.elapsedTime;

        List<Vector3> pathNodes = new List<Vector3>();
        for (int i = 0; i < numberOfNodes; i++)
        {
            pathNodes.Add(path[i]);
            if (i < numberOfNodes - 1)
            {
                pathLength += Vector3.Distance(path[i], path[i + 1]);
            }
        }
        pathCreator.bezierPath = new BezierPath(pathNodes, false, PathSpace.xyz);
        speed = pathLength / elapsedTime;
     
    }
    public void SetSpeed()
    {
        ghostRacer.pathFollower.speed = speed;
    }


}
