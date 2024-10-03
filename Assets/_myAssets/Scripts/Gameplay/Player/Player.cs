using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int coins = 100;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Update()
    {
        coinsText.text = ("Coins: " + coins);
    }
}
