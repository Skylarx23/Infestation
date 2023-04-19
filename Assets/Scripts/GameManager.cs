using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnScript.SpawnEnemies(4, Enemy);
    }

    // Update is called once per frame
    void Update()
    {
        // Why is this here?
        // HealthBar.value--;
    }
}
