using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Slider HealthBar;
    public TextMeshProUGUI healthText;

    public AudioSource soundSource;
    public AudioSource alertSource;
    public AudioSource playerSource;
    public AudioClip healingSound;
    public AudioClip queenFightMusic, queenFightMusic2;
    public AudioClip idleMusic1;
    public AudioClip fightTheme;
    public AudioClip ambience1, ambience2;
    public AudioClip alertSound;
    public AudioClip queenSpawn;
    public AudioClip defeatClip;
    public AudioClip endFightTheme;

    private AiScript aiScript;

    private float damageAmount;
    public float QueenHealth;
    private bool isDead = false;
    private bool queenSpawned = false;

    bool hasMedKit = true;
    public GameObject MedIcon;
    public GameObject RifleUI;
    public GameObject PistolUI;
    public GameObject queenSpawner;
    public GameObject alertUI;
    public GameObject damageUI;
    public GameObject damageAcidUI;
    public GameObject queenUI;
    public Slider queenSlider;
    public GameObject UI;
    public GameObject defeatUI;
    public Text defeatText;

    public GameObject startRoom;
    public GameObject firstWave;
    public GameObject endRoom;
    public GameObject firstWaveDoor1;
    public GameObject firstWaveDoor2;
    public GameObject endRoomDoor;
    public GameObject room1RunnerSpawner;
    public GameObject room1DroneSpawner;
    public GameObject room1WarriorSpawner;
    public GameObject endRoomRunnerSpawner;
    public GameObject endRoomDroneSpawner;
    public GameObject endRoomWarriorSpawner;
    public GameObject endRoomGuardSpawner;

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
        queenUI.SetActive(false);
        defeatUI.SetActive(false);
        UI.SetActive(true);
        endRoom.SetActive(false);
        endRoomDoor.SetActive(false);
    }

    private void Update()
    {
        healthText.text = HealthBar.value.ToString();
        queenSlider.value = QueenHealth;
        //if (Input.GetKeyUp(KeyCode.K)) StartCoroutine(WaveTest());
        if (HealthBar.value <= 0 && isDead == false)
        {
            PlayerDeath();
            isDead = true;
        }
    }

    public IEnumerator DamagePlayer(float damage, GameObject Enemy)
    {
        yield return new WaitForSeconds(1f);
        if (Enemy.GetComponent<AiScript>().Attackable == true)
        {
            HealthBar.value -= damage;
            yield return new WaitForSeconds(0.25f);
            damageUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            damageUI.SetActive(false);
        }
    }

    public IEnumerator DamagePlayerHazard(float damage)
    {
        Debug.Log("hazard damage");
        HealthBar.value -= damage;
        yield return new WaitForSeconds(0.25f);
        damageUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        damageUI.SetActive(false);
    }

    public void HealPlayer()
    {
        if (hasMedKit)
        {
            HealthBar.value += 1000;
            hasMedKit = false;
            playerSource.PlayOneShot(healingSound);
            //MedIcon.SetActive(false);
            MedIcon.transform.GetComponent<Image>().color = new Vector4(190, 33, 33, 0.1f);
        }
    }

    public void CollectMedKit(GameObject MedKit)
    {
        if (!hasMedKit)
        {
            hasMedKit = true;
            Destroy(MedKit);
            //MedIcon.SetActive(true);
            MedIcon.transform.GetComponent<Image>().color = new Vector4(190, 33, 33, 1f);
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

    private IEnumerator DisableAlert()
    {
        yield return new WaitForSeconds(3f);
        alertUI.SetActive(false);
    }

    public IEnumerator SpawnQueen()
    {
        queenSpawned = true;
        alertSource.clip = queenSpawn;
        alertSource.volume = 0.05f;
        alertSource.Play();
        yield return new WaitForSeconds(2f);
        soundSource.clip = queenFightMusic;
        soundSource.volume = 0.07f;
        soundSource.Play();
        yield return new WaitForSeconds(2f);
        GameObject Model = queenSpawner.GetComponent<SpawnScript>().Model;
        queenSpawner.GetComponent<SpawnScript>().SpawnEnemies(1, Model);
        queenUI.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator QueenDeath()
    {
        UI.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        SceneManager.LoadScene("QueenDeath");
        
    }

    public void QueenPhase2()
    {
        soundSource.clip = queenFightMusic2;
        soundSource.volume = 0.07f;
        soundSource.Play();
    }

    public void PlayerDeath()
    {
        Time.timeScale = 0f;
        UI.SetActive(false);
        defeatUI.SetActive(true);
        soundSource.clip = defeatClip;
        soundSource.volume = 0.07f;
        soundSource.Play();
    }

    public void FirstRoom()
    {
        firstWaveDoor1.SetActive(true);
        startRoom.SetActive(false);
        StartCoroutine(WavesFirstRoom());
    }

    public void EndRoom()
    {
        endRoom.SetActive(true);
    }

    public void EndRoom2()
    {
        endRoomDoor.SetActive(true);
        firstWave.SetActive(false);
        StartCoroutine(EndRoomWaves());
    }

    //public IEnumerator WaveTest()
    //{
        //for (int i = 0; i < 5; i++)
        //{
            //Debug.Log("Wave" + i);
            // Grabs the Enemy prefab from the spawner and tries to spawn it X amount of times
            //GameObject Model = Wave1Spawners[Wave1Spawners.Length].GetComponent<SpawnScript>().Model;
           // Wave1Spawners[Random.Range(0, Wave1Spawners.Length)].GetComponent<SpawnScript>().SpawnEnemies(i * 2, Model);

            // Waits until all Enemies are dead 
           // yield return new WaitUntil(() => Wave1Spawners[i].GetComponent<SpawnScript>().Enemies.Count == 0);
            //Debug.Log("Wave" + i + " Finished");
        //}
    //}

    public IEnumerator WavesFirstRoom()
    {
        // written by Skylar
        // warning: wall of text incoming
        // the spawn enemies could have been made a function but I have one massive excuse (read below)
        soundSource.clip = fightTheme;
        soundSource.volume = 0.06f;
        soundSource.PlayDelayed(1f);
        yield return new WaitForSeconds(1.5f);
        
        SpawnRunner(1, 3);
        yield return new WaitUntil(() => room1RunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        
        yield return new WaitForSeconds(2f);
        SpawnRunner(1,5);
        yield return new WaitUntil(() => room1RunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(2f);
        SpawnRunner(1,2);
        SpawnWarrior(1,1);
        yield return new WaitUntil(() => room1RunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => room1WarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(2f);
        SpawnDrone(1,1);
        yield return new WaitUntil(() => room1DroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        
        yield return new WaitForSeconds(2f);
        SpawnWarrior(1,3);
        yield return new WaitUntil(() => room1WarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(2f);
        SpawnRunner(1,6);
        SpawnWarrior(1,1);

        yield return new WaitForSeconds(10f);
        SpawnDrone(1,2);
        yield return new WaitUntil(() => room1RunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => room1WarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => room1DroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(2f);
        SpawnRunner(1,6);
        SpawnWarrior(1,2);

        yield return new WaitForSeconds(20f);
        SpawnDrone(1,2);

        yield return new WaitForSeconds(30f);
        SpawnWarrior(1,1);

        yield return new WaitUntil(() => room1RunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => room1WarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => room1DroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        soundSource.Stop();
        firstWaveDoor2.SetActive(false);
        // the excuse is that i am lazy and i already wrote half of it xd
    }

    public void SpawnRunner(int stage, int amount)
    {
        GameObject ModelRunner = room1RunnerSpawner.GetComponent<SpawnScript>().Model;
        GameObject ModelRunnerEnd = endRoomRunnerSpawner.GetComponent<SpawnScript>().Model;
        
        if(stage == 1)
        {
            room1RunnerSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelRunner);
        }
        else if(stage == 2)
        {
            endRoomRunnerSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelRunnerEnd);
        }
    }

      public void SpawnDrone(int stage, int amount)
    {
        GameObject ModelDrone = room1DroneSpawner.GetComponent<SpawnScript>().Model;
        GameObject ModelDroneEnd = endRoomDroneSpawner.GetComponent<SpawnScript>().Model;
        
        if(stage == 1)
        {
            room1DroneSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelDrone);
        }
        else if(stage == 2)
        {
            endRoomDroneSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelDroneEnd);
        }
        alertUI.SetActive(true);
        alertSource.clip = alertSound;
        alertSource.Play();
        StartCoroutine(DisableAlert());
    }

      public void SpawnWarrior(int stage, int amount)
    {
        GameObject ModelWarrior = room1WarriorSpawner.GetComponent<SpawnScript>().Model;
        GameObject ModelWarriorEnd = endRoomWarriorSpawner.GetComponent<SpawnScript>().Model;

        if(stage == 1)
        {
            room1WarriorSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelWarrior);
        }
        else if(stage == 2)
        {
            endRoomWarriorSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelWarriorEnd);
        }
    }

      public void SpawnGuard(int amount)
    {
        GameObject ModelGuard = endRoomGuardSpawner.GetComponent<SpawnScript>().Model;
        endRoomGuardSpawner.GetComponent<SpawnScript>().SpawnEnemies(amount, ModelGuard);
    }

    public IEnumerator EndRoomWaves()
    {
        soundSource.clip = endFightTheme;
        soundSource.volume = 0.07f;
        soundSource.Play();
        yield return new WaitForSeconds(1.5f);
        
        SpawnWarrior(2,2);
        yield return new WaitUntil(() => endRoomWarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(3f);
        SpawnRunner(2,20);
        yield return new WaitForSeconds(15f);
        SpawnDrone(2,2);
        yield return new WaitUntil(() => endRoomDroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => endRoomRunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(3f);
        SpawnGuard(1);
        yield return new WaitUntil(() => endRoomGuardSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(3f);
        SpawnGuard(2);
        SpawnWarrior(2,1);
        yield return new WaitUntil(() => endRoomWarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => endRoomGuardSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);

        yield return new WaitForSeconds(10f);  
        SpawnRunner(2,10);
        SpawnWarrior(2,2);
        SpawnGuard(1);

        yield return new WaitForSeconds(15f);
        SpawnDrone(2,2);

        yield return new WaitForSeconds(10f);
        SpawnRunner(2,8);

        yield return new WaitForSeconds(10f);
        SpawnRunner(2,8);

        yield return new WaitForSeconds(30f); 
        SpawnGuard(1);

        yield return new WaitUntil(() => endRoomRunnerSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => endRoomWarriorSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => endRoomDroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitUntil(() => endRoomGuardSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        yield return new WaitForSeconds(10f);

        StartCoroutine(SpawnQueen());
        if(queenSpawned == false)
        {
            StartCoroutine(SpawnQueen());
        }

        yield return new WaitForSeconds(30f);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(20f);
            SpawnDrone(2,3);
            yield return new WaitUntil(() => endRoomDroneSpawner.GetComponent<SpawnScript>().Enemies.Count == 0);
        }
    }
}