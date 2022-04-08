using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GarageManager : MonoBehaviour
{
    public Transform displayAnchor; // the garage display anchor
    public MainMenuUIController mainMenuUIController; // the main menu ui controller
    public PrefabGarage prefabGarage; // the prefab garage
    public enum KartBody{Kart0, Kart1} // the types of kart bodies
    public enum Character{Character0, Character1} // the types of character
    public int currentKartBodyIndex; // the currently shown kart body's index
    public int currentCharacterIndex; // the currently shown character's index
    public GameObject currentKartOnDisplay; // the current kart on display gameobject
    public GameObject currentCharacterOnDisplay; // the current character on display gameobject
    public List<GameObject> spawnedKartBodiesForDisplay; // the list of spawned kart bodies for display gameobjects
    public List<GameObject> spawnedCharactersForDisplay; // the list of spawned characters for display gameobjects
    public bool rotate; // the status of wether or not the garage is currently rotating

    private void Start()
    {
        Initialize(); // initialize the script
    }

    public void Initialize()
    {
        mainMenuUIController = FindObjectOfType<MainMenuUIController>(); // store a local reference to the main menu ui controller
        mainMenuUIController.nextCharacterButton.onClick.AddListener(delegate { IncrementCurrentCharacterDisplayIndex();}); // add the listener for the next character button
        mainMenuUIController.previousCharacterButton.onClick.AddListener(delegate { DecrementCurrentCharacterDisplayIndex(); }); // add the listener for the previous character button
        mainMenuUIController.nextKartBodyButton.onClick.AddListener(delegate { IncrementCurrentKartDisplayIndex(); }); // add the listener for the kart body button
        mainMenuUIController.previousKartBodyButton.onClick.AddListener(delegate { DecrementCurrentKartDisplayIndex(); }); // add the listener for the previous kart body button
        currentCharacterIndex = PlayerPrefs.GetInt("ChosenCharacterIndex"); // set the current character index from the playerprefs
        currentKartBodyIndex =  PlayerPrefs.GetInt("ChosenKartBodyIndex"); // set the current kart body from the player prefs
        SpawnDisplayPrefabs(); // spawn the prefabs for display
    }

    public void FixedUpdate()
    {
        if (rotate) // check if the garage display should be rotating
        {
            currentKartOnDisplay.transform.Rotate(new Vector3(0f, 1f, 0f)); // rotate the kart body on display
            currentCharacterOnDisplay.transform.Rotate(new Vector3(0f, 1f, 0f)); // rotate the character on display
        }
        currentCharacterOnDisplay.GetComponentInChildren<Animator>().SetFloat("Blend", 0.5f); // set the animation blend tree value to 0.5f (idle neutral)
    }

    public void ToggleRotate()
    {
        rotate = !rotate; // toggle the rotation flag
    }

    public void SpawnDisplayPrefabs()
    {
        displayAnchor = GameObject.FindGameObjectWithTag("GarageDisplayAnchor").transform; // store a local reference to the garage display anchor
        spawnedKartBodiesForDisplay = new List<GameObject>(); // create a new list that will store the spawned kart b odies on display
        for(int i = 0; i < prefabGarage.kartBodyPrefabs.Count; i++) // for each kart body prefab in the prefab garage
        {
            GameObject go = GameObject.Instantiate(prefabGarage.kartBodyPrefabs[i], displayAnchor); // instantiate the kart body at the display anchor
            foreach (TrailRenderer tr in go.GetComponentsInChildren<TrailRenderer>()) // for each trail renderer of the kart body
            {
                tr.gameObject.SetActive(false); // disable the trail renderer
            }
            foreach (VisualEffect ve in go.GetComponentsInChildren<VisualEffect>()) // for each visual effect of the kart body
            {
                ve.gameObject.SetActive(false); // disable the visual effect
            }
            foreach (ParticleSystem ps in go.GetComponentsInChildren<ParticleSystem>()) // for each particle system of the kart body
            {
                ps.gameObject.SetActive(false); // disable the particle system
            }
            foreach (AudioSource aus in go.GetComponents<AudioSource>()) // for each audio source in the kart body
            {
                aus.volume = 0f; // set the volume of the audio system to zero
            }
            spawnedKartBodiesForDisplay.Add(go); // add the spawned kart body to the list of spawned kart bodes for display
        }
        spawnedCharactersForDisplay = new List<GameObject>(); // create a new list of characters for display
        for (int i = 0; i < prefabGarage.characterPrefabs.Count; i++) // for each character prefab in the prefab garage
        {
            GameObject go = GameObject.Instantiate(prefabGarage.characterPrefabs[i], displayAnchor); // instantiate the character prefab at the display anchor
            spawnedCharactersForDisplay.Add(go); // add the spawned character tothe list of spawned characters for display
        }
        UpdatePrefabActiveStatus(); // update the status of the active prefab on display
    }

    public void UpdatePrefabActiveStatus()
    {
        for (int i = 0; i < spawnedCharactersForDisplay.Count; i++) // for each spawned character for display
        {
            spawnedCharactersForDisplay[i].gameObject.SetActive(false); // disable the spawned character game object
        }
        for (int i = 0; i < spawnedKartBodiesForDisplay.Count; i++) // for each spawned kart body for display
        {
            spawnedKartBodiesForDisplay[i].gameObject.SetActive(false); // disable the spawned kart body for display
        }
        currentCharacterOnDisplay = spawnedCharactersForDisplay[currentCharacterIndex]; // set the current character on display
        currentCharacterOnDisplay.SetActive(true); // activate the current character on display
        currentKartOnDisplay = spawnedKartBodiesForDisplay[currentKartBodyIndex]; // set the current kart on display
        currentKartOnDisplay.SetActive(true); // activate the current kart on display
        currentKartOnDisplay.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f)); // set the rotation of the current kart on display
        currentCharacterOnDisplay.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f)); // set the rotation of the current character on display
        mainMenuUIController.garageCharacterText.text = currentCharacterOnDisplay.name.Replace("(Clone)", "").Trim(); // remove the clone tag from the gameobject name
        mainMenuUIController.garageKartText.text = currentKartOnDisplay.name.Replace("(Clone)", "").Trim(); // remove the clóne tag from the gameobject name
    }

    public void IncrementCurrentKartDisplayIndex()
    {
        currentKartBodyIndex++; // increment the current kart body index
        if (currentKartBodyIndex > spawnedKartBodiesForDisplay.Count -1) // check if we are at the end of the array
        {
            currentKartBodyIndex = 0; // cycle back to the start of the array
        }
        PlayerPrefs.SetInt("ChosenKartBodyIndex", currentKartBodyIndex); // update the player prefs for ChosenKartBodyIndex
        PlayerPrefs.Save(); // save the player prefs
        UpdatePrefabActiveStatus(); // update the prefab active status
    }

    public void IncrementCurrentCharacterDisplayIndex()
    {
        currentCharacterIndex++; // increment the current character index
        if (currentCharacterIndex > spawnedCharactersForDisplay.Count -1) // check if we are at the end of the array
        {
            currentCharacterIndex = 0; // cycle back to the start of the array
        }
        PlayerPrefs.SetInt("ChosenCharacterIndex", currentCharacterIndex); // update the play prefs for ChosenCharacterIndex
        PlayerPrefs.Save(); // save the player prefs
        UpdatePrefabActiveStatus(); // update the prefab active status
    }
    public void DecrementCurrentCharacterDisplayIndex()
    {
        currentCharacterIndex--; // decrement the current character index
        if (currentCharacterIndex < 0) // check if we are the start of the array
        {
            currentCharacterIndex = spawnedCharactersForDisplay.Count-1; // cycle back to the end of the array
        }
        PlayerPrefs.SetInt("ChosenCharacterIndex", currentCharacterIndex); // update the player prefs for chosen ChosenCharacterIndex 
        PlayerPrefs.Save(); // save the player prefs
        UpdatePrefabActiveStatus(); // update the prefab active status
    }
    public void DecrementCurrentKartDisplayIndex()
    {
        currentKartBodyIndex--; // decrement the current kart body index
        if (currentKartBodyIndex < 0) // check if we are at the start of the array
        {
            currentKartBodyIndex = spawnedKartBodiesForDisplay.Count-1; // cycle back to the end of the array
        }
        PlayerPrefs.SetInt("ChosenKartBodyIndex", currentKartBodyIndex); // update the player prefs for ChosenKartBodyIndex
        PlayerPrefs.Save(); // save the player prefs
        UpdatePrefabActiveStatus(); // update the prefab active status
    }
}
