using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject boatPosition;

    private void Update()
    {
        this.gameObject.transform.position = boatPosition.transform.position;
        this.gameObject.transform.rotation = boatPosition.transform.rotation;
    }
}
