using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public AudioSource[] sources;
    bool isAlive;
    public GameObject queenUI;
    public GameObject queen;
    // Start is called before the first frame update
    void Start()
    {
        isAlive = false;
        sources = GetComponents<AudioSource>();
    }

    public void SpawnBoss()
    {
        queenUI.gameObject.SetActive(true);
        sources[0].Play();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.G))
        {
            SpawnBoss();
        }
    }
}

