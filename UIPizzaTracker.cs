using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinishLine;
using UnityEngine.UI;

public class UIPizzaTracker : MonoBehaviour
{
    public Track track;
    public Image[] pizzaImages;

    private void Start()
    {
        pizzaImages = GetComponentsInChildren<Image>();
        UpdatePizzaStatus();
    }

    public void UpdatePizzaStatus()
    {
        Collectibles collectibles = SaveSystem.LoadCollectibles();
        bool[] pizzas;
        collectibles.registry.TryGetValue(track.ToString(), out pizzas);

        for (int i = 0; i < 8; i++)
        {
            pizzaImages[i].gameObject.SetActive(pizzas[i]);
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
