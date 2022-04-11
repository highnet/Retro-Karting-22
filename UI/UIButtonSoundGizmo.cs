using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundGizmo : MonoBehaviour
{

    public UIAudioManager UIAudioManager;
 

    // Start is called before the first frame update
    void Start()
    {

        UIAudioManager = GameObject.FindWithTag("UIAudioManager").GetComponent<UIAudioManager>();

     //   buttonImage.gu

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void PlayRandomClickSound() {
        UIAudioManager.PlayRandomButtonClickSound();
    }

    public void PlayRandomHoverSound()
    {
        UIAudioManager.PlayRandomButtonHoverSound();
    }

    public void PlayRandomBackSound()
    {
        UIAudioManager.PlayRandomButtonBackSound();
    }
}
