using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    public RespawnPoint[] respawnPoints;

    private void Start()
    {
        respawnPoints = GetComponentsInChildren<RespawnPoint>();
    }


    public GameObject GetFurthrestRespawnPoint()
    {
        for(int i = respawnPoints.Length-1; i >= 0; i--)
        {
            if (respawnPoints[i].active)
            {
                return respawnPoints[i].respawnLocation;
            }
        }
        return respawnPoints[0].respawnLocation;
    }
}
