using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class EndOfRacePanel : MonoBehaviour
{
    public TextMeshProUGUI progressSavedText;
    public float progressSavedTextAlpha;
    public TextMeshProUGUI totalRaceTime;
    public TextMeshProUGUI lap1Time;
    public TextMeshProUGUI lap2Time;
    public TextMeshProUGUI lap3Time;
    public TextMeshProUGUI goldTime;
    public TextMeshProUGUI goldName;
    public TextMeshProUGUI silverTime;
    public TextMeshProUGUI silverName;
    public TextMeshProUGUI bronzeTime;
    public TextMeshProUGUI bronzeName;
    public Image characterThumbnail;
    public Image kartThumbnail;
    public FinishLine finishLine;
    public PrefabGarage prefabGarage;
    public RaceController raceController;
    public Color goldColor;
    public Color silverColor;
    public Color bronzeColor;


    // Start is called before the first frame update
    private void Start()
    {
        finishLine = GameObject.FindGameObjectWithTag("Finish").GetComponent<FinishLine>();
        prefabGarage = FindObjectOfType<PrefabGarage>();
        raceController = FindObjectOfType<RaceController>();
    }

    // Update is called once per frame
    void Update()
    {
        progressSavedText.color = new Color(1f, 1f, 1f, progressSavedTextAlpha);
    }

    public void PrepareEndOfRaceScreen()
    {
        RecordEntry myRecordEntry = finishLine.recordEntry;
        DOTween.To(() => progressSavedTextAlpha, (newValue) => progressSavedTextAlpha = newValue, 0f, 600f).SetEase(Ease.InOutFlash, 300f, 0f).SetLoops(-1, LoopType.Restart);
        totalRaceTime.text = TimeSpan.FromSeconds(myRecordEntry.time).ToString(@"hh\:mm\:ss");
        characterThumbnail.sprite = prefabGarage.characterThumbnailSprites[(int) myRecordEntry.character];
        kartThumbnail.sprite = prefabGarage.kartBodyThumbnailSprites[(int)myRecordEntry.kart];
        List<RecordEntry> recordEntries = finishLine.recordEntries;
        List<RecordEntry> myPersonalRecordEntries = new List<RecordEntry>(recordEntries);
        myPersonalRecordEntries.RemoveAll(name => name.author != "You");
        if (raceController.lapTimes.Count >= 1)
        {
            lap1Time.text = TimeSpan.FromSeconds(raceController.lapTimes[0]).ToString(@"hh\:mm\:ss");
        }
        if (raceController.lapTimes.Count >= 2)
        {
            lap2Time.text = TimeSpan.FromSeconds(raceController.lapTimes[1]).ToString(@"hh\:mm\:ss");
        }
        if (raceController.lapTimes.Count >= 3)
        {
            lap3Time.text = TimeSpan.FromSeconds(raceController.lapTimes[2]).ToString(@"hh\:mm\:ss");
        }
        goldTime.text = TimeSpan.FromSeconds(recordEntries[0].time).ToString(@"hh\:mm\:ss");
        goldName.text = recordEntries[0].author;
        silverTime.text = TimeSpan.FromSeconds(recordEntries[1].time).ToString(@"hh\:mm\:ss");
        silverName.text = recordEntries[1].author;
        bronzeTime.text = TimeSpan.FromSeconds(recordEntries[2].time).ToString(@"hh\:mm\:ss");
        bronzeName.text = recordEntries[2].author;
    }
}
