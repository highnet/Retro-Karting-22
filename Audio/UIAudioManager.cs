using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{

    public AudioClipPlayer clipPlayer;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public  void PlayRandomButtonClickSound()
    {
        clipPlayer.PlayOneShot(0, Random.Range(0, 10), 1.0f, true);
    }

    public void PlayRandomButtonHoverSound()
    {
        clipPlayer.PlayOneShot(0, Random.Range(10,20), 1.0f, true);
    }

    public void PlayRandomButtonBackSound()
    {
        clipPlayer.PlayOneShot(0, Random.Range(20, 25), 1.0f, true);
    }
}
