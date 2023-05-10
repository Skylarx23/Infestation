using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    bool isAlive;
    public GameObject queenUI;
    public GameObject queen;


    // Start is called before the first frame update
    void Start()
    {
        isAlive = false;
    }

    public void SpawnBoss()
    {
        queenUI.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            SpawnBoss();
        }
    }
}

