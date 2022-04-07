using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualityDropdown : MonoBehaviour
{
    public  TMP_Dropdown dropdown; // the dropdown
    private UserSettings userSettings; // the user's settings

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>(); // store a local reference to the dropdown
        userSettings = GameObject.FindGameObjectWithTag("User Settings").GetComponent<UserSettings>(); // get a local reference to the user's settings
        QualitySettings.SetQualityLevel(userSettings.qualityLevel, true); // set the quality settings based on the user's settings
        dropdown.value = userSettings.qualityLevel; // update the dropdown value
        dropdown.RefreshShownValue(); // refresh the dropdown value
    }

    public void ChangeQuality()
    {
        PlayerPrefs.SetInt("QualityLevel", dropdown.value); // set the player prefs for "QualityLevel" based on the dropdown value
        PlayerPrefs.Save(); // save the player prefs
        userSettings.qualityLevel = dropdown.value; // set the user settings quality level based on the dropdown value
        QualitySettings.SetQualityLevel(userSettings.qualityLevel, true); // set the quality settings based on the user's settings
    }
}
