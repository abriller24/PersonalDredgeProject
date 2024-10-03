using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishTrigger: MonoBehaviour
{
    public bool beingCaught = false;
    private FishingMiniGame minigameController;

    private void Start()
    {
        minigameController = FindFirstObjectByType<FishingMiniGame>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (minigameController.reelingFish)
        {
            if (other.CompareTag("CatchingBar") && !beingCaught)
            {
                beingCaught = true;
                minigameController.FishInBar();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CatchingBar") && beingCaught)
        {
            beingCaught = false;
            minigameController.FishOutOfBar();
        }
    }
}