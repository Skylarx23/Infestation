using System.Collections;
using UnityEngine;

public class CPTrigger : MonoBehaviour
{
    GameManager GM;

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Whenever the Player enters the trigger it'll send a signal to the GM 
    private void OnTriggerEnter(Collider Collision)
    {
        if (Collision.gameObject.tag == "Player") GM.StartTest();
        Destroy(gameObject);
    }
}
