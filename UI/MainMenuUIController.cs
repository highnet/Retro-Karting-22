using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using static FinishLine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public Image background; // the background image
    public float currentBackgroundImageAlpha; // the current RBGA alpha of the background image
    public float startBackgroundImageAlpha; // the starting RBHGA alpha of the background image
    public Mixer mixerMain; // the game's main mixer
    public TextMeshProUGUI garageCharacterText; // the garage's character text in the garage panel
    public TextMeshProUGUI garageKartText; // the garage's kart text in the garage panel
    public UIPanelMovement masterPanel; // the master panel
    public int rankingDisplayedIndex; // the index of the current track shown on display in the ranking panel
    public TextMeshProUGUI[] rankingTimes; // the array of texts of ranking times in the ranking panel
    public TextMeshProUGUI[] rankingAuthors; // the array of text of ranking authors in the ranking panel
    public Image[] rankingTitles; // the array of images of ranking titles in the ranking panel
    public Image[] rankingCharacterThumbnails; // the array of images of ranking characters in the ranking panel
    public Image[] rankingKartBodyThumbnails; // the array of images of ranking kart bodies in the ranking panel
    public PrefabGarage prefabGarage; // the prefab farage
    public Button nextCharacterButton; // the next character button in the garage panel
    public Button previousCharacterButton; // the previous character button in the garage panel
    public Button nextKartBodyButton; // the next kart button in the garage panel
    public Button previousKartBodyButton; // the previous kart button in the garage panel
    public Image changeLogTextImage; // the changelog text image
    public GameObject changeLog; // the changelog game object

    // Start is called before the first frame update
    void Start()
    {
        prefabGarage = FindObjectOfType<PrefabGarage>(); // store a local reference to the prefab garage
        currentBackgroundImageAlpha = startBackgroundImageAlpha; // set the current background image alpha to the starting background image alpha
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>(); // the game's main mixer
        UpdateMutedStatus(); // update the game's muted status
        UpdateRankingsDisplay(); // update the ranking displays
    }

    private void Update()
    {
        background.color = new Color32(255, 255, 255, (byte)currentBackgroundImageAlpha); // update the background color alpha
    }

    public void ToggleMutedStatus()
    {
        mixerMain.ToggleMutedStatus();  // toggle the muted status
    }

    public void UpdateMutedStatus()
    {
        mixerMain.UpdateMutedStatus(); // update the muted status
    }

    public void ActivateChangeLogTextImage()
    {
        changeLogTextImage.gameObject.SetActive(true); // activate the changelog text image
    }
    public void DeactivateChangeLogTextImage()
    {
        changeLogTextImage.gameObject.SetActive(false); // deactivate the changelog text image
    }

    public void ActivateChangelog()
    {
        changeLog.gameObject.SetActive(!changeLog.gameObject.activeSelf); // activate the changelog gameobject
    }

    public void DeactivateChangeLog()
    {
        changeLog.gameObject.SetActive(false); // deactivate the changelog gameobject
    }

    public void UpdateRankingsDisplay()
    {
        Records records = SaveSystem.LoadRecords(); // load the records from the save system
        List<RecordEntry> entries; // create a new record entry list
        records.registry.TryGetValue(((Track) rankingDisplayedIndex).ToString(), out entries); // get the records for the index of the current track shown on display in the ranking panel
        for (int i = 0; i < (entries.Count > 8 ? 8 : entries.Count); i++) // for the top 8 or less entries in the record
        {
            rankingTimes[i].text = string.Format("{0:N2}", entries[i].time) + "s"; // update the ranking times text with the appropiate time
            rankingAuthors[i].text = entries[i].author; // update the ranking author text with the appropiate author
            if (entries[i].author == "You") // if the entries author is the user himself
            {
                rankingTimes[i].color = Color.red; // set the time text color red
                rankingAuthors[i].color = Color.red; // set the author text color red
            } else // otherwise
            {
                rankingTimes[i].color = Color.white; // set the time text color white 
                rankingAuthors[i].color = Color.white; // set the author text color white
            }
            rankingCharacterThumbnails[i].color = new Color(1f, 1f, 1f, 1f); // set the character thumbnail image as fully visible
            rankingKartBodyThumbnails[i].color = new Color(1f, 1f, 1f, 1f); // set the  kart thumbnail image as fully visible
            rankingCharacterThumbnails[i].sprite = prefabGarage.characterThumbnailSprites[(int)entries[i].character]; // set the character thumbnail image with the appropiate character of the record entry
            rankingKartBodyThumbnails[i].sprite = prefabGarage.kartBodyThumbnailSprites[(int)entries[i].kart]; // set the kart thumbnail image with the appropiate character of the record entry
        }
        for(int i = 0; i < rankingTitles.Length; i++) // for each track ranking title
        {
            rankingTitles[i].gameObject.SetActive(false); // deactivate the track ranking title
        }
        rankingTitles[rankingDisplayedIndex].gameObject.SetActive(true); // activate only the appropiate track ranking title
    }

    public void IncrementRankingDisplayedIndex()
    {
        rankingDisplayedIndex++; // increment the ranking display index
        if (rankingDisplayedIndex > rankingTitles.Length - 1) // check if we are at the end of the array
        {
            rankingDisplayedIndex = 0; // cycle back to the beginning of the array
        }
        UpdateRankingsDisplay(); // update the rankings display
    }

    public void DecrementRankingDisplayedIndex()
    {
        rankingDisplayedIndex--; // decrement the ranking display index
        if (rankingDisplayedIndex < 0) // check if we are the start of the array
        {
            rankingDisplayedIndex = rankingTitles.Length - 1; // cycle back to the end of the array
        }
        UpdateRankingsDisplay(); // update the rankings display
    }
}
