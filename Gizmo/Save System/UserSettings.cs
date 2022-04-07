using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSettings : MonoBehaviour
{

    public float masterVolume; // the master volume
    public float musicVolume; // the music volume
    public float sfxVolume; // the sfx volume
    public bool soundMuted; // the sound muted status
    public int qualityLevel; // the quality level
    public bool retroFilterOn; // the retro filter status
    public bool hintsAndTips; // the hints and tips status

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this); // flag this script not to be destroyed on scene changes
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f); // get the player prefs for MasterVolume, if it does not exist yet, use the default value of 1
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);// get the player prefs for MusicVolume, if it does not exist yet, use the default value of 1
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);// get the player prefs for SFXVolume, if it does not exist yet, use the default value of 1
        soundMuted = PlayerPrefs.GetFloat("SoundMuted?", 0.0f) == 1.0f;// get the player prefs for SoundMuted?, if it does not exist yet, use the default value of 0
        qualityLevel = PlayerPrefs.GetInt("QualityLevel", 0);// get the player prefs for QualityLevel, if it does not exist yet, use the default value of 0
        retroFilterOn = PlayerPrefs.GetFloat("RetroFilterOn?", 1.0f) == 1.0f;// get the player prefs for RetroFilterOn?, if it does not exist yet, use the default value of 1
        hintsAndTips = PlayerPrefs.GetFloat("HintsAndTips?", 1.0f) == 1.0f;// get the player prefs for HintsAndTips?, if it does not exist yet, use the default value of 1
    }

}
