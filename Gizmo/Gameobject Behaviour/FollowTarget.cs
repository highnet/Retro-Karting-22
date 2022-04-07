using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public enum FollowerType {Minimap} // the types of target followers

    public GameObject player; // the game's player object
    public float cameraDistance; // the distance to follow from
    public FollowerType followerType; // the type of target follower

    void LateUpdate()
    {
        if (followerType == FollowerType.Minimap) // minimap target follower
        {
            transform.position = player.transform.position + new Vector3(0.0f, cameraDistance, 0.0f); // teleport to the player position with an additional offset
        }
    }
}
