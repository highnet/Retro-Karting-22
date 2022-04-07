using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuKartController : MonoBehaviour
{
    public Animator animator; // the animator
    void Start()
    {
        animator.SetFloat("Blend", 0.5f); // set animator "Blend" float value to 0.5
    }
}
