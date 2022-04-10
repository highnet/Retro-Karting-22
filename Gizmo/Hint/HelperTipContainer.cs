using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperTipContainer : MonoBehaviour
{

    private string[] floatingHelperTipstexts; // the array of strings of possible helper tip
    private List<FloatingHelperTip> floatingHelperTips; // the list of floating helper tips
    private float lifeTime = 40f; // the life time of each floating helper tips
    public bool alive = true; // the status boolean of the helper tips
    public UserSettings userSettings; // the user's settings

    void Start()
    {
        floatingHelperTipstexts = new string[] { // generate the array of strings of possible helper tips
                                            "When driving on curves, press \"CTRL\" to enter a drift",
                                            "Drive over the red arrows to get a speed boost!",
                                            "Press \"ENTER\" whenever you wish to consume a speed boost",
                                            "Collect as many coins as you can, as they make you drive faster",
                                            "The longer you drift, the faster boost you receive as the end of the drift",
                                            "Steering decreases your velocity, plan ahead to drive straight as much as possible!",
                                            "Pressing \"SPACE\" will cause your kart to brake ",
                                            "Press \"L\" to toggle your kart's front lights",
                                            "Take different routes each lap in order to earn more coins",
                                            "Slow down when driving on curvy sections to avoid falling off the track",
                                            "Pressing and holding the throttle two seconds before the countdown ends will earn you a free speed boost"
        };
        userSettings = FindObjectOfType<UserSettings>(); // store a local reference to the user's settings
        if (!userSettings.hintsAndTips) // check if the user has chosen they dont want to see hints and tips
        {
            floatingHelperTipstexts = new string[] { "" }; // empty the helper tips
        }
        floatingHelperTips = new List<FloatingHelperTip>(FindObjectsOfType<FloatingHelperTip>()); // populate the list of floating helper tips
    }

    void Update()
    {
        if (alive) // if the floating helper tips are still alive
        {
            lifeTime -= Time.deltaTime; // deduct from the timer
            if (lifeTime < 0)
            { // if the timer has reached below zero

                alive = false; // set the status to dead
                foreach (FloatingHelperTip fht in floatingHelperTips) // for each floating helper tip
                {
                    fht.gameObject.SetActive(false); // disable the floating helper tip
                }
            }
        }
    }

    public string GetRandomHelperTip() // gets a random helper tip
    {
        return floatingHelperTipstexts[UnityEngine.Random.Range(0, floatingHelperTipstexts.Length)]; // get a random helper tip out of all possible helper tips
    }
}
