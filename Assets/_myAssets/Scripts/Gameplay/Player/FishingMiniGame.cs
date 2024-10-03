using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishingMiniGame : MonoBehaviour
{
    private Player player;
    private bool lineCast = false;
    private bool nibble = false;
    public bool reelingFish = false;

    [SerializeField] private GameObject catchingbar;
    private Vector3 catchingBarLoc;
    private Rigidbody2D catchingBarRB;

    [SerializeField] private GameObject fishBar;
    [SerializeField] private Sprite fishImage;
    private bool inTrigger = false;

    private float catchPercentage = 0f;
    [SerializeField] private Slider catchProgressBar; 

    [SerializeField] private GameObject minigameCanvas;

    [SerializeField] private KeyCode fishingKey = KeyCode.Space; 
    [SerializeField] private float catchMultiplier = 10f; 
    [SerializeField] private float catchingForce; 

    private void Start()
    {
        player = GetComponent<Player>();
        catchingBarRB = catchingbar.GetComponent<Rigidbody2D>(); //Get reference to the Rigidbody on the catchingbar
        catchingBarLoc = catchingbar.GetComponent<RectTransform>().localPosition; //Use this to reset the catchingbars position to the bottom of the "water"
    }

    private void Update()
    {
        if (!reelingFish)
        { //If we arent currently in the reeling stage
            if (Input.GetKeyDown(fishingKey) && !lineCast)
            { //This is if we are doing nothing and are ready to cast a line
                CastLine();
            }
            else if (Input.GetKeyDown(fishingKey) && lineCast && nibble)
            { //This is if we reel in while there is a nibble
                StopAllCoroutines(); //Stops the LineBreak timer
                StartReeling();
            }
        }
        else
        { //This is when we are in the stage where we are fighitng for the fish
            if (Input.GetKey(fishingKey))
            { //If we press space
                catchingBarRB.AddForce(Vector2.up * catchingForce * Time.deltaTime, ForceMode2D.Force); //Add force to lift the bar
            }
        }

        //If the fish is in our trigger box
        if (inTrigger && reelingFish)
        {
            catchPercentage += catchMultiplier * Time.deltaTime;
        }
        else
        {
            catchPercentage -= catchMultiplier * Time.deltaTime;
        }

        //Clamps our percentage between 0 and 100
        catchPercentage = Mathf.Clamp(catchPercentage, 0, 100);
        catchProgressBar.value = catchPercentage;
        if (catchPercentage >= 100)
        { //Fish is caught if percentage is full
            catchPercentage = 0;
            FishCaught();
        }
    }

    //Called to cast our line
    private void CastLine()
    {
        lineCast = true;
        StartReeling();
    }

    //Used to start the minigame
    private void StartReeling()
    {
        reelingFish = true;

        nibble = false;
        lineCast = false;

        fishBar.GetComponent<Image>().sprite = fishImage;

        minigameCanvas.SetActive(true);
    }

    //Called from the FishTrigger script
    public void FishInBar()
    {
        inTrigger = true;
    }

    //Called from the FishTrigger script
    public void FishOutOfBar()
    {
        inTrigger = false;
    }

    //Called when the catchpercentage hits 100
    public void FishCaught()
    {
        Debug.Log("Caught a: Bass");
        reelingFish = false; //No longer reeling in a fish

        minigameCanvas.SetActive(false); //Disable the fishing canvas
        catchingbar.transform.localPosition = catchingBarLoc; //Reset the catching bars position
        player.coins += Random.Range(5, 20);
    }

}