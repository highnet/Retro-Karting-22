using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInstancer : MonoBehaviour
{
    public GameObject canvasPrefab; // the prefab of the game's canvas

    private void Start()
    {
        GameObject canvas = GameObject.Instantiate(canvasPrefab); // instantiate a new canvas game object
        RacingUIController mainMenuUIController = canvas.GetComponent<RacingUIController>(); // get the canvas racing ui controller
        mainMenuUIController.raceController = FindObjectOfType<RaceController>(); // set the race controller
        mainMenuUIController.kartController = FindObjectOfType<KartController>(); // set the kart controller
        mainMenuUIController.finishArchAudioClipPlayer = FindObjectOfType<FinishLine>().audioClipPlayer; // set the finish arch audio clip player
    }
}
