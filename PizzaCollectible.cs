using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FinishLine;
using DG.Tweening;

public class PizzaCollectible : MonoBehaviour
{
    public Track track;
    public int collectibleID;
    public Achievements achievements;

    private void Start()
    {
        Collectibles collectibles = SaveSystem.LoadCollectibles();
        if (collectibles.registry[track.ToString()][collectibleID])
        {
            this.transform.Translate(new Vector3(1000f, 1000f, 1000f)); // move the powerup trigger off map
        }
        transform.DOLocalRotate(new Vector3(0, 0, 360), 5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);

        achievements = FindObjectOfType<Achievements>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collectibles collectibles = SaveSystem.LoadCollectibles();
            bool[] pizzas = collectibles.registry[track.ToString()];
            pizzas[collectibleID] = true;
            SaveSystem.SaveCollectibles(collectibles);
            this.transform.Translate(new Vector3(1000f, 1000f, 1000f)); // move the powerup trigger off map

            int numberOfPizzas = 0;
            for(int i = 0; i < pizzas.Length; i++)
            {
                if (pizzas[i])
                {
                    numberOfPizzas++;
                }
            }
            achievements.SetPizzas((int) track, numberOfPizzas);

        }
    }
}
