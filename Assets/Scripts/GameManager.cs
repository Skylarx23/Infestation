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

    bool hasMedKit = true;
    public GameObject MedIcon;
    public GameObject RifleUI;
    public GameObject PistolUI;


    // Start is called before the first frame update
    void Start()
    {
        aiScript = FindObjectOfType<AiScript>();
        //SpawnScript.SpawnEnemies(4, Enemy);
        soundSource.clip = ambience1;
        soundSource.volume = 0.15f;
        soundSource.Play();

        MedIcon.SetActive(true);
    }

    public IEnumerator DamagePlayer(float damage, GameObject Enemy)
    {
        yield return new WaitForSeconds(1.5f);
        if (Enemy.GetComponent<AiScript>().isInRange == true) HealthBar.value -= damage;
    }

    public void HealPlayer()
    {
        if (hasMedKit)
        {
            HealthBar.value += 750;
            hasMedKit = false;
            MedIcon.SetActive(false);
        }
    }

    public void CollectMedKit(GameObject MedKit)
    {
        if (!hasMedKit)
        {
            hasMedKit = true;
            Destroy(MedKit);
            MedIcon.SetActive(true);
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
        soundSource.Play();
        // You can put whatever you want in here; spawing enemies, playing music, etc.
        // Each Trigger point should have its own function to tell it what to do
    }
}
