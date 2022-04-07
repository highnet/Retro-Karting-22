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
    public Image background;
    public float currentBackgroundImageAlpha;
    public float startBackgroundImageAlpha;
    public Mixer mixerMain;

    public TextMeshProUGUI garageCharacterText;
    public TextMeshProUGUI garageKartText;

    public UIPanelMovement masterPanel;

    public int rankingDisplayedIndex;
    public TextMeshProUGUI[] rankingTimes;
    public TextMeshProUGUI[] rankingAuthors;
    public Image[] rankingTitles;
    public Image[] rankingCharacterThumbnails;
    public Image[] rankingKartBodyThumbnails;

    public PrefabGarage prefabGarage;

    public Button nextCharacterButton;
    public Button previousCharacterButton;
    public Button nextKartBodyButton;
    public Button previousKartBodyButton;

    public Image changeLogTextImage;

    public GameObject changeLog;



    // Start is called before the first frame update
    void Start()
    {

        prefabGarage = FindObjectOfType<PrefabGarage>();
        currentBackgroundImageAlpha = startBackgroundImageAlpha;
        mixerMain = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>();
        UpdateMutedStatus();
        UpdateRankingsDisplay();


    }

    private void Update()
    {
        background.color = new Color32(255, 255, 255, (byte)currentBackgroundImageAlpha);

    }

    public void ToggleMutedStatus()
    {
        mixerMain.ToggleMutedStatus();  
    }

    public void UpdateMutedStatus()
    {
        mixerMain.UpdateMutedStatus();
    }

    public void ActivateChangeLogTextImage()
    {
        changeLogTextImage.gameObject.SetActive(true);
    }
    public void DeactivateChangeLogTextImage()
    {
        changeLogTextImage.gameObject.SetActive(false);
    }

    public void ActivateChangelog()
    {
        changeLog.gameObject.SetActive(!changeLog.gameObject.activeSelf);
    }

    public void DeactivateChangeLog()
    {
        changeLog.gameObject.SetActive(false);
    }

    public void UpdateRankingsDisplay()
    {
        Records records = SaveSystem.LoadRecords();
        List<RecordEntry> entries;
        records.registry.TryGetValue(((Track) rankingDisplayedIndex).ToString(), out entries);
        for(int i = 0; i < (entries.Count > 8 ? 8 : entries.Count); i++)
        {
            rankingTimes[i].text = string.Format("{0:N2}", entries[i].time) + "s";
            rankingAuthors[i].text = entries[i].author;
            if (entries[i].author == "You")
            {
                rankingTimes[i].color = Color.red;
                rankingAuthors[i].color = Color.red;
            } else
            {
                rankingTimes[i].color = Color.white;
                rankingAuthors[i].color = Color.white;
            }
            rankingCharacterThumbnails[i].color = new Color(1f, 1f, 1f, 1f);
            rankingKartBodyThumbnails[i].color = new Color(1f, 1f, 1f, 1f);
            rankingCharacterThumbnails[i].sprite = prefabGarage.characterThumbnailSprites[(int)entries[i].character];
            rankingKartBodyThumbnails[i].sprite = prefabGarage.kartBodyThumbnailSprites[(int)entries[i].kart];

        }

        for(int i = 0; i < rankingTitles.Length; i++)
        {
            rankingTitles[i].gameObject.SetActive(false);
        }
        rankingTitles[rankingDisplayedIndex].gameObject.SetActive(true);

    }

    public void IncrementRankingDisplayedIndex()
    {
        rankingDisplayedIndex++;

        if (rankingDisplayedIndex > rankingTitles.Length - 1)
        {
            rankingDisplayedIndex = 0;
        }
        UpdateRankingsDisplay();
    }

    public void DecrementRankingDisplayedIndex()
    {
        rankingDisplayedIndex--;
        if (rankingDisplayedIndex < 0)
        {
            rankingDisplayedIndex = rankingTitles.Length - 1;
        }
        UpdateRankingsDisplay();

    }


}
