using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinishLine;
using UnityEngine.UI;
using TMPro;

public class UIPizzaTracker : MonoBehaviour
{
    public Track track;
    public Image[] pizzaImages;
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        pizzaImages = GetComponentsInChildren<Image>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        UpdatePizzaStatus();
    }

    public void UpdatePizzaStatus()
    {
        Collectibles collectibles = SaveSystem.LoadCollectibles();
        bool[] pizzas;
        collectibles.registry.TryGetValue(track.ToString(), out pizzas);

        int numberOfPizzas = 0;
        for (int i = 0; i < 8; i++)
        {
            if (pizzas[i])
            {
                numberOfPizzas++;
            }
            pizzaImages[i].gameObject.SetActive(pizzas[i]);

        }
        textMesh.text = numberOfPizzas + "/8";

        if (numberOfPizzas == 8)
        {
            textMesh.text = "<i>" + textMesh.text + "</i>";
        }
    }

    public void ResetProgess()
    {
        Collectibles collectibles = SaveSystem.LoadCollectibles();
        collectibles.registry[track.ToString()] = new bool[] { false, false, false, false, false, false, false, false };
        SaveSystem.SaveCollectibles(collectibles);
        UpdatePizzaStatus();
    }
}
