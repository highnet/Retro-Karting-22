using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintsAndTipsDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown; // the dropwdown
    private UserSettings userSettings; // the user's settings

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>(); // store a local reference to the dropdown
        userSettings = GameObject.FindGameObjectWithTag("User Settings").GetComponent<UserSettings>(); // store a local reference to the user's settings
        dropdown.value = userSettings.hintsAndTips ? 0 : 1; // update the dropdown value based on the user's settings of hints and tips
        dropdown.RefreshShownValue(); // refresh the shown value on the dropdown
    }

    public void ChangeHintsAndTipsPreference()
    {
        PlayerPrefs.SetFloat("HintsAndTips?", dropdown.value == 0 ? 1.0f : 0.0f); // set the playerprefs HintsAndTips? value based on the dropdown value
        PlayerPrefs.Save(); // save the player prefs
        userSettings.hintsAndTips = dropdown.value == 0 ? true : false; // update the user's settings on hints and tips based on the dropdown value

    }
}
