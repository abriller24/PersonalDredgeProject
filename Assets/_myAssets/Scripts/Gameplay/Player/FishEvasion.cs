using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishEvasion : MonoBehaviour
{
    
    //The max and min heights we can go
    [SerializeField] private RectTransform maxHeight;
    [SerializeField] private RectTransform minHeight;
    
    [Range(0, 5f)]public float moveSpeed; //How fast the fish move
    public float maxWaitTime, minWaitTime; //How long the fish waits before moving again
    
    private Vector3 currentDestination; //Where the fish is moving towards

    private bool waiting = false; //Used when waiting for a new destination

    private void Start() 
    {
        currentDestination = RandomDestination(); //Give the fish a random direction to go to
    }

    private void Update() 
    {
        transform.position = Vector3.Lerp(transform.position, currentDestination, moveSpeed  * Time.deltaTime); 
        
        if (Vector3.Distance(transform.position, currentDestination) <= 1f && !waiting) 
        {
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait() 
    { //Used to let the fish wait a bit before getting a new destination
        waiting = true;
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        currentDestination = RandomDestination();
        waiting = false;
    }

    private Vector3 RandomDestination() 
    {
        var rectT = GetComponent<RectTransform>();
        var maxUp = maxHeight.position.y - rectT.sizeDelta.y/2;
        var maxDown = minHeight.position.y + rectT.sizeDelta.y/2;
        var newHeight = Random.Range(maxUp, maxDown);

        return new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}
