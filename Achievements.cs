using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class Achievements : MonoBehaviour
{

    private CGameID gameID;

    protected Callback<UserStatsReceived_t> userStatsReceived;

    private int track0pizzas;
    private int track1pizzas;

    void Start()
    {
        SteamUserStats.RequestCurrentStats();

        if (SteamManager.Initialized)
        {
            userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        }
    }

    public void SetPizzas(int trackID,int numberOfPizzas)
    {
        Debug.Log("[SteamStats] Rewarding Pizzas (trackID: " + trackID + " , numberOfPizzas: " + numberOfPizzas + ")");
        switch (trackID)
        {
            case 0:
                SteamUserStats.SetStat("Track 0 Pizzas", numberOfPizzas);
                break;
            case 1:
                SteamUserStats.SetStat("Track 1 Pizzas", numberOfPizzas);
                break;
        }
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        if ((ulong)gameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                SteamUserStats.GetStat("Track 0 Pizzas", out track0pizzas);
                SteamUserStats.GetStat("Track 1 Pizzas", out track1pizzas);

                Debug.Log("[SteamStats] " + track0pizzas + " Track 0 Pizzas");
                Debug.Log("[SteamStats] " + track1pizzas + " Track 1 Pizzas");


            } else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }


        }
    }


}
