using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;

    public AudioSource soundSource;
    public AudioClip queenFightMusic;
    public AudioClip idleMusic1;
    public AudioClip fightTheme;
    public AudioClip ambience1, ambience2;

    private AiScript aiScript;
    private float damageAmount;

    // Start is called before the first frame update
    void Start()
    {
        aiScript = FindObjectOfType<AiScript>();
        //SpawnScript.SpawnEnemies(4, Enemy);
        soundSource.clip = ambience1;
        soundSource.Play();
    }

    public IEnumerator DamagePlayer(float damage)
    {

        yield return new WaitForSeconds(1.5f);
        damageAmount = damage;
        AttackPlayer();
    }

    public void AttackPlayer()
    {
        Debug.Log(aiScript.isInRange);
        if (aiScript.isInRange == true)
        {
            HealthBar.value -= damageAmount;
        }
        else
        {
            return;
        }
    }

    public void StartTest()
    {
        Debug.Log("Test Started!");
        soundSource.clip = fightTheme;
        soundSource.volume = 0.01f;
        soundSource.Play();
        // You can put whatever you want in here; spawing enemies, playing music, etc.
        // Each Trigger point should have its own function to tell it what to do
    }
}
