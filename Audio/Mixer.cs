using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UISlider;

public class Mixer : MonoBehaviour
{
    public AudioMixer main; // the audio mixer
    private UserSettings userSettings; // the user's settings
    private float minimumDB = -80f; // the minimum decibels that all sounds will go to
    private float masterMaximumDB; // the maximum decibels the master sounds mix will go to
    private float musicMaximumDB; // the maximum decibels the music sounds mix will go to
    private float environmentMaximumDB; // the maximum decibels the environment sounds will go to
    private float carMaximumDB; // the maximum decibels the car sounds will go to
    private float fxMaximumDB; // the maximum decibels the fx sounds will go to
    private float characterMaximumDB; // the maximum decibels character sounds will go to
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this); // set the script to persist through scene changes
        userSettings = GameObject.FindGameObjectWithTag("User Settings").GetComponent<UserSettings>(); // store a local reference to the user's settings
        main.GetFloat("Master", out masterMaximumDB); // get unity's mixer'smaster volume 
        main.GetFloat("Music", out musicMaximumDB); // get unity's mixer's music volume
        main.GetFloat("Environment", out environmentMaximumDB); // get unity's mixer's environment volume
        main.GetFloat("Car", out carMaximumDB); // get unity's mixer's car volume
        main.GetFloat("FX", out fxMaximumDB); // get unity's mixer's audio mixer's fx volume
        main.GetFloat("Character", out characterMaximumDB); // get unity's mixer's character volume
        UpdateVolumes(); // update the mixer's volume

    }

    public void UpdateVolumes()
    {        
        // update volumes from player prefs, the player prefs are set by the user with UI sliders, for simplicity, the user can only control master volume, music volume and sfx volume, so environment/car/fx/character are all bundled into the sfx volume slider
        main.SetFloat("Master", Remap((Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, masterMaximumDB)); // set unity's mixer's master volume in decibels according to a logarithmic scale
        main.SetFloat("Music", Remap((Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, musicMaximumDB)); // set unity's mixer's music volume in decibels according to a logarithmic scale
        main.SetFloat("Environment", Remap((Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, environmentMaximumDB)); // set unity's mixer's sfx volume in decibels according to a logarithmic scale
        main.SetFloat("Car", Remap((Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, carMaximumDB)); // set unity's mixer's car volume in decibels according to a logarithmic scale
        main.SetFloat("FX", Remap((Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, fxMaximumDB)); // set unity's mixer's master volume in decibels according to a logarithmic scale
        main.SetFloat("Character", Remap((Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1.0f)) * minimumDB / -2f), minimumDB, 0f, minimumDB, characterMaximumDB)); // set unity's mixer's master volume in decibels according to a logarithmic scale

    }

    public void UpdateVolumes(SliderType sliderType, float value)
    {
        // update volumes with the UI sliders, for simplicity, the user can only control master volume, music volume and sfx volume, so environment/car/fx/character are all bundled into the sfx volume slider
        switch (sliderType) // UI slider fork
        {
            case SliderType.MASTERVOLUME: // master volume slider
                main.SetFloat("Master", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, masterMaximumDB)); // set unity's mixer's master volume in decibels according to a logarithmic scale
                PlayerPrefs.SetFloat("MasterVolume", value); // set the master volume player pref
                break;
            case SliderType.MUSICVOLUME: // music volume slider
                main.SetFloat("Music", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, musicMaximumDB));// set unity's mixer's music volume in decibels according to a logarithmic scale
                PlayerPrefs.SetFloat("MusicVolume", value); // set the music volume player pref
                break;
            case SliderType.SFXVOLUME: // sfx volume slider
                main.SetFloat("Environment", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, environmentMaximumDB)); // set unity's mixer's environment volume in decibels according to a logarithmic scale
                main.SetFloat("Car", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, carMaximumDB)); // set unity's mixer's car volume in decibels according to a logarithmic scale
                main.SetFloat("FX", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, fxMaximumDB)); // set unity's mixer's fx volume in decibels according to a logarithmic scale
                main.SetFloat("Character", Remap((Mathf.Log10(value) * minimumDB / -2f), minimumDB, 0f, minimumDB, characterMaximumDB)); // set unity's mixer's character volume in decibels according to a logarithmic scale
                PlayerPrefs.SetFloat("SFXVolume", value); // set the sfxvolume player pref
                break;
        }

        PlayerPrefs.Save(); // save the player prefs

    }

    public float Remap(float value, float from1, float to1, float from2, float to2) 
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2; // linear ratio remap a value from the range [from1,to1] to [from2,to2]
    }

    public void ToggleMutedStatus()
    {
        userSettings.soundMuted = !userSettings.soundMuted; // toggle the user's settings' sound muted variable
        UpdateMutedStatus(); // update the mixer according to the new muted status
    }

    public void UpdateMutedStatus()
    {
        PlayerPrefs.SetFloat("SoundMuted?", userSettings.soundMuted ? 1 : 0); // set the sound muted player pref
        PlayerPrefs.Save(); // save the player prefs
        AudioListener.volume = userSettings.soundMuted ? 0 : 1; // set the audio listener volume according to the new muted status 1 for audible, 0 for inaudible
    }
}
