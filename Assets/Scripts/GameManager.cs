using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;
    public Text healthText;

    public AudioSource soundSource;
    public AudioSource alertSource;
    public AudioSource playerSource;
    public AudioClip healingSound;
    public AudioClip queenFightMusic;
    public AudioClip idleMusic1;
    public AudioClip fightTheme;
    public AudioClip ambience1, ambience2;
    public AudioClip alertSound;
    public AudioClip queenSpawn;

    private AiScript aiScript;

    private float damageAmount;

    bool hasMedKit = true;
    public GameObject MedIcon;
    public GameObject RifleUI;
    public GameObject PistolUI;
    public GameObject droneSpawner;
    public GameObject queenSpawner;
    public GameObject WaveSpawner;
    public GameObject alertUI;
    public GameObject damageUI;
    public GameObject damageAcidUI;

    // Start is called before the first frame update
    void Start()
    {
        aiScript = FindObjectOfType<AiScript>();
        //SpawnScript.SpawnEnemies(4, Enemy);
        soundSource.clip = ambience1;
        soundSource.volume = 0.1f;
        soundSource.Play();
        //MedIcon.SetActive(true);
        alertUI.SetActive(false);
        damageUI.SetActive(false);
        damageAcidUI.SetActive(false);
    }

    private void Update()
    {
        healthText.text = HealthBar.value.ToString();

        if (Input.GetKeyUp(KeyCode.K)) StartCoroutine(WaveTest());
    }

    public IEnumerator DamagePlayer(float damage, GameObject Enemy)
    {
        yield return new WaitForSeconds(0.5f);
        if (Enemy.GetComponent<AiScript>().isInRange == true)
        {
            HealthBar.value -= damage;
            yield return new WaitForSeconds(0.25f);
            damageUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            damageUI.SetActive(false);
        }
    }

    public void DamagePlayerHazard(float damage)
    {
        Debug.Log("hazard damage");
        HealthBar.value -= damage;
    }

    public void HealPlayer()
    {
        if (hasMedKit)
        {
            HealthBar.value += 1000;
            hasMedKit = false;
            playerSource.PlayOneShot(healingSound);
            //MedIcon.SetActive(false);
            MedIcon.transform.GetComponent<Image>().color = new Vector4(1, 1, 1, 0.2f);
        }
    }

    public void CollectMedKit(GameObject MedKit)
    {
        if (!hasMedKit)
        {
            hasMedKit = true;
            Destroy(MedKit);
            //MedIcon.SetActive(true);
            MedIcon.transform.GetComponent<Image>().color = new Vector4(0, 0, 0, 1f);
        }
    }

    public void GunUI(GameObject GunController)
    {

        // Changes weapon icon to 30% tranparency or 100% depending if its selected and displays its ammo
        GunSwitcher GSwich = GameObject.Find("GunController").GetComponent<GunSwitcher>();
        if (GSwich.SelectedWeapon == 0)
        {
            RifleUI.transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
            PistolUI.transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 1, 1, 0.3f);
        }
        else
        {
            RifleUI.transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 1, 1, 0.3f);
            PistolUI.transform.GetChild(0).GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
        }
    }

    public void StartTest()
    {
        Debug.Log("Test Started!");
        soundSource.clip = fightTheme;
        soundSource.volume = 0.06f;
        soundSource.PlayDelayed(2.5f);
        alertSource.clip = alertSound;
        alertSource.Play();
        GameObject Model = droneSpawner.GetComponent<SpawnScript>().Model;
        droneSpawner.GetComponent<SpawnScript>().SpawnEnemies(1, Model);
        alertUI.SetActive(true);
        StartCoroutine(DisableAlert());
        // You can put whatever you want in here; spawing enemies, playing music, etc.
        // Each Trigger point should have its own function to tell it what to do
    }

    public IEnumerator WaveTest()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Wave" + i);
            // Grabs the Enemy prefab from the spawner and tries to spawn it X amount of times
            GameObject Model = droneSpawner.GetComponent<SpawnScript>().Model;
            WaveSpawner.GetComponent<SpawnScript>().SpawnEnemies(i, Model);

            // Waits until all Enemies are dead 
            yield return new WaitUntil(() => WaveSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
            Debug.Log("Wave" + i + " Finished");
        }
    }

    private IEnumerator DisableAlert()
    {
        yield return new WaitForSeconds(3f);
        alertUI.SetActive(false);
    }

    public IEnumerator SpawnQueen()
    {
        soundSource.clip = queenFightMusic;
        soundSource.volume = 0.07f;
        alertSource.clip = queenSpawn;
        alertSource.volume = 0.05f;
        alertSource.Play();
        Debug.Log("sc theme");
        soundSource.Play();
        GameObject Model = queenSpawner.GetComponent<SpawnScript>().Model;
        queenSpawner.GetComponent<SpawnScript>().SpawnEnemies(1, Model);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator QueenDeath()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
