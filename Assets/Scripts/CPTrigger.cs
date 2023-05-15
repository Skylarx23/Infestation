using System.Collections;
using UnityEngine;

public class CPTrigger : MonoBehaviour
{
    GameManager GM;
    public bool isQueenSpawner = false;
    public bool isDroneSpawner = false;
    public bool isCheckpoint1 = false;
    public bool isCheckpoint2 = false;
    public bool isCheckpoint3 = false;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Whenever the Player enters the trigger it'll send a signal to the GM 
    private void OnTriggerEnter(Collider Collision)
    {
        if (isDroneSpawner == true && Collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        //if (isQueenSpawner == true && Collision.gameObject.tag == "Player")
        //{
            //StartCoroutine(GM.SpawnQueen());
            //Destroy(gameObject);
        //}

        if (isCheckpoint1 == true && Collision.gameObject.tag == "Player")
        {
            GM.FirstRoom();
            Destroy(gameObject);
        }

        if (isCheckpoint2 == true && Collision.gameObject.tag == "Player")
        {
            GM.EndRoom2();
            Destroy(gameObject);
        }

        if (isCheckpoint3 == true && Collision.gameObject.tag == "Player")
        {
            GM.EndRoom();
            Destroy(gameObject);
        }


    }
}
