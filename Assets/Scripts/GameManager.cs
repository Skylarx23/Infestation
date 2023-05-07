using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;
    public GameObject enemy;

    public AudioSource soundSource;
    public AudioClip queenFightMusic;
    public AudioClip idleMusic1;
    public AudioClip fightTheme;
    public AudioClip ambience1, ambience2;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnScript.SpawnEnemies(4, Enemy);
        soundSource.PlayOneShot(idleMusic1, 0.3f);
    }

    public void DamagePlayer(float damage)
    {
        HealthBar.value -= damage;
    }
}
