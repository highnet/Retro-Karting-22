using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlider : MonoBehaviour
{
    public enum SliderType { NONE, MASTERVOLUME, MUSICVOLUME, SFXVOLUME } // determines what type of slider this is
    public enum SourceType { NONE, USERSETTINGS } // determines what menu panel the slider lives on (aka its source)
    public SourceType sourceType; // the slider's source type
    public SliderType sliderType; // the slider's type
    public TextMeshProUGUI text; // the text number of the slider, ranging from 0 to 100
    public Mixer mixerMain; // the game's main mixer
    private UserSettings userSettings; // the game's user's settings
    private Slider slider; // the UI slider element
    void Start()
    {
        slider = GetComponent<Slider>(); // store a local reference to the slider
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>(); // store a local reference to the game's audio mixer
        switch (sourceType) // source type fork
        {
            case SourceType.USERSETTINGS: // main menu user settings panel
                userSettings = GameObject.FindGameObjectsWithTag("User Settings")[0].GetComponent<UserSettings>(); // store a local reference to the user's settings
                break;
        }

        if (userSettings) // if user settings exists
        {
            switch (sliderType) // slider type fork
            {
                case SliderType.MASTERVOLUME: // master volume slider
                     slider.value = userSettings.masterVolume; // update the sliders value to reflect the user settings
                    break;
                case SliderType.MUSICVOLUME: // music volume slider
                    slider.value = userSettings.musicVolume; // update the sliders value to reflect the user settings
                    break;
                case SliderType.SFXVOLUME: // sfx volume slider
                    slider.value = userSettings.sfxVolume; // update the sliders value to reflect the user settings
                    break;
            }
            UpdateText(); // update the slider text
        }
    }

    public void UpdateValue()
    {
        if (userSettings) // if usersettings exists
        {
            switch (sliderType) // slider type fork
            {
                case SliderType.MASTERVOLUME: // master volume slider
                    userSettings.masterVolume = slider.value; // update the user's settings with slider's current value
                    break;
                case SliderType.MUSICVOLUME: // music volume slider
                    userSettings.musicVolume = slider.value; // update the user's settings with slider's current value
                    break;
                case SliderType.SFXVOLUME: // sfx volume slider
                    userSettings.sfxVolume = slider.value; // update the user's settings with slider's current value
                    break;
            }
            UpdateText(); // update the sliders text
            UpdateVolumes(); // update the mixer values

        }
    }

    public void UpdateText()
    {
        text.text = Mathf.RoundToInt((slider.value * 100f)).ToString() ; //update the sliders text, convert the float which runs from [0.0f to 1.0f] to [0 to 100]
    }

    public void UpdateVolumes()
    {
        mixerMain.UpdateVolumes(sliderType,slider.value); // update the mixer values
    }
}
