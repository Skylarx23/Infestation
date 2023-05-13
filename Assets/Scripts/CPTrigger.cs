using System.Collections;
using UnityEngine;

public class CPTrigger : MonoBehaviour
{
    GameManager GM;
    public bool isQueenSpawner = false;
    public bool isDroneSpawner = false;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Whenever the Player enters the trigger it'll send a signal to the GM 
    private void OnTriggerEnter(Collider Collision)
    {
        if (isDroneSpawner == true && Collision.gameObject.tag == "Player")
        {
            GM.StartTest();
            Destroy(gameObject);
        }
        if (isQueenSpawner == true && Collision.gameObject.tag == "Player")
        {
            StartCoroutine(GM.SpawnQueen());
            Destroy(gameObject);
        }

    }
}
