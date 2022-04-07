using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GarageManager : MonoBehaviour
{
    public Transform displayAnchor;
    public MainMenuUIController mainMenuUIController;
    public PrefabGarage prefabGarage;
    public enum KartBody
    {
        Kart0, Kart1
    }

    public enum Character
    {
       Character0, Character1
    }

    public int currentKartBodyIndex;
    public int currentCharacterIndex;

    public GameObject currentKartOnDisplay;
    public GameObject currentCharacterOnDisplay;

    public List<GameObject> spawnedKartBodiesForDisplay;
    public List<GameObject> spawnedCharactersForDisplay;

    public bool rotate;


    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {


        mainMenuUIController.nextCharacterButton.onClick.AddListener(delegate { IncrementCurrentCharacterDisplayIndex();});
        mainMenuUIController.previousCharacterButton.onClick.AddListener(delegate { DecrementCurrentCharacterDisplayIndex(); });
        mainMenuUIController.nextKartBodyButton.onClick.AddListener(delegate { IncrementCurrentKartDisplayIndex(); });
        mainMenuUIController.previousKartBodyButton.onClick.AddListener(delegate { DecrementCurrentKartDisplayIndex(); });

        currentCharacterIndex = PlayerPrefs.GetInt("ChosenCharacterIndex");
        currentKartBodyIndex =  PlayerPrefs.GetInt("ChosenKartBodyIndex");

        SpawnDisplayPrefabs();
    }

    public void FixedUpdate()
    {
        if (rotate)
        {
            currentKartOnDisplay.transform.Rotate(new Vector3(0f, 1f, 0f));
            currentCharacterOnDisplay.transform.Rotate(new Vector3(0f, 1f, 0f));
        }
        currentCharacterOnDisplay.GetComponentInChildren<Animator>().SetFloat("Blend", 0.5f);

    }

    public void ToggleRotate()
    {
        rotate = !rotate;
    }

    public void SpawnDisplayPrefabs()
    {
        displayAnchor = GameObject.FindGameObjectWithTag("GarageDisplayAnchor").transform;
        spawnedKartBodiesForDisplay = new List<GameObject>();
        for(int i = 0; i < prefabGarage.kartBodyPrefabs.Count; i++)
        {
            GameObject go = GameObject.Instantiate(prefabGarage.kartBodyPrefabs[i], displayAnchor);
            foreach (TrailRenderer tr in go.GetComponentsInChildren<TrailRenderer>())
            {
                tr.gameObject.SetActive(false);
            }

            foreach (VisualEffect ve in go.GetComponentsInChildren<VisualEffect>())
            {
                ve.gameObject.SetActive(false);
            }

            foreach (ParticleSystem ps in go.GetComponentsInChildren<ParticleSystem>())
            {
                ps.gameObject.SetActive(false);
            }

            foreach (AudioSource aus in go.GetComponents<AudioSource>())
            {
                aus.volume = 0f;
            }
            spawnedKartBodiesForDisplay.Add(go);


        }

        spawnedCharactersForDisplay = new List<GameObject>();
        for (int i = 0; i < prefabGarage.characterPrefabs.Count; i++)
        {
            GameObject go = GameObject.Instantiate(prefabGarage.characterPrefabs[i], displayAnchor);
            spawnedCharactersForDisplay.Add(go);
        }
        UpdatePrefabActiveStatus();
    }

    public void UpdatePrefabActiveStatus()
    {

        for (int i = 0; i < spawnedCharactersForDisplay.Count; i++)
        {
            spawnedCharactersForDisplay[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < spawnedKartBodiesForDisplay.Count; i++)
        {
            spawnedKartBodiesForDisplay[i].gameObject.SetActive(false);
        }

        currentCharacterOnDisplay = spawnedCharactersForDisplay[currentCharacterIndex];
        currentCharacterOnDisplay.SetActive(true);
        currentKartOnDisplay = spawnedKartBodiesForDisplay[currentKartBodyIndex];
        currentKartOnDisplay.SetActive(true);

        currentKartOnDisplay.transform.rotation = Quaternion.Euler(new Vector3(0f,90f,0f));
        currentCharacterOnDisplay.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

        mainMenuUIController = FindObjectOfType<MainMenuUIController>();
        mainMenuUIController.garageCharacterText.text = currentCharacterOnDisplay.name.Replace("(Clone)", "").Trim();
        mainMenuUIController.garageKartText.text = currentKartOnDisplay.name.Replace("(Clone)", "").Trim();



    }


    public void IncrementCurrentKartDisplayIndex()
    {
        currentKartBodyIndex++;
        if (currentKartBodyIndex > spawnedKartBodiesForDisplay.Count -1)
        {
            currentKartBodyIndex = 0;
        }
        PlayerPrefs.SetInt("ChosenKartBodyIndex", currentKartBodyIndex);
        PlayerPrefs.Save();

        UpdatePrefabActiveStatus();
    }

    public void IncrementCurrentCharacterDisplayIndex()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex > spawnedCharactersForDisplay.Count -1)
        {
            currentCharacterIndex = 0;
        }
        PlayerPrefs.SetInt("ChosenCharacterIndex", currentCharacterIndex);
        PlayerPrefs.Save();

        UpdatePrefabActiveStatus();
    }
    public void DecrementCurrentCharacterDisplayIndex()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = spawnedCharactersForDisplay.Count-1;
        }
        PlayerPrefs.SetInt("ChosenCharacterIndex", currentCharacterIndex);
        PlayerPrefs.Save();
        UpdatePrefabActiveStatus();
    }
    public void DecrementCurrentKartDisplayIndex()
    {
        currentKartBodyIndex--;
        if (currentKartBodyIndex < 0)
        {
            currentKartBodyIndex = spawnedKartBodiesForDisplay.Count-1;
        }
        PlayerPrefs.SetInt("ChosenKartBodyIndex", currentKartBodyIndex);
        PlayerPrefs.Save();
        UpdatePrefabActiveStatus();
    }
}
