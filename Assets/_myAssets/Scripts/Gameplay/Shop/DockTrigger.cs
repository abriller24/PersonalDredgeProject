using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DockTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject interactionMenu;
    [SerializeField] private Player player;
    [SerializeField] private BoatBlueprint boats;

    private void Start()
    {
        player = GetComponentInChildren<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        interactionMenu.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        interactionMenu.SetActive(false);
    }

    private void Update()
    {
        if(interactionMenu.active == true)
        {
            if (Input.GetKey(KeyCode.F))
            {
                shopMenu.SetActive(true);
            }
        }
    }
    public void OnNextBoatButton()
    {
        boats.NextBoat();
    }

    public void OnPreviousBoatButton()
    {
        boats.PreviousBoat();
    }


}
