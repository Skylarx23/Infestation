using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public bool SpamClick = false;

    public float Range = 100;
    public float Damage = 10;
    public float FireRate = 10;
    private float NextTimetoFire = 0;
    public bool Reloading, isRunning;

    public int AmmoMax = 20;
    private int Ammo;
    public float ReloadTime = 2;
    public Text AmmoText;

    public AudioClip shootClip, reloadClip;
    public AudioSource soundSource;

    public Camera PlayerCam;
    public ParticleSystem MuzzleFlash;
    public Light Flash;
    public GameObject BulletHole;
    public GameObject AcidBlood;

    public Animator animationSource;
    public PlayerMovement PM;

    private void Awake()
    {
        PM = GameObject.Find("First Person Player").GetComponent<PlayerMovement>();

        Ammo = AmmoMax;
        Ammo--;
        Ammo++;
        UpdateText();
    }

    private void Update()
    {

        // if you're already trying to reload it stops everything untill you're done
        if (Reloading) return;

        // Checks to see if R key has been pressed or if you're out of Ammo and then reloads
        if (Input.GetKeyUp(KeyCode.R) || Ammo <= 0) StartCoroutine(Reload());
        else
        {
            // if you're running it stops you from shooting
            if (PM.isRunning) return;

            if (SpamClick)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= NextTimetoFire)
                {
                    // Slows fire rate to 1/Firerate, so if FireRate = 10 then you can only fire every 0.1s
                    NextTimetoFire = Time.time + 1f / FireRate;
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time >= NextTimetoFire)
                {
                    // Slows fire rate to 1/Firerate, so if FireRate = 10 then you can only fire every 0.1s
                    NextTimetoFire = Time.time + 1f / FireRate;
                    Shoot();
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        Reloading = true;
        Ammo = 0;
        AmmoText.text = "Reloading!";
        animationSource.SetTrigger("trReload");
        soundSource.PlayOneShot(reloadClip, 0.5f);
        yield return new WaitForSeconds(ReloadTime);
        Ammo = AmmoMax;
        UpdateText();
        Reloading = false;
    }

    private IEnumerator MuzzleLight()
    {
        Flash.enabled = true;
        yield return new WaitForSeconds(0.01f);
        Flash.enabled = false;
    }

    public void UpdateText()
    {
        AmmoText.text = this.gameObject.name + ": " + Ammo + "/" + AmmoMax;
    }

    void Shoot()
    {
        animationSource.SetTrigger("trShoot");
        
        RaycastHit hit;

        // Updates the ammo and the Ammo Text
        Ammo--;
        UpdateText();

        // Plays partical system and shines light
        MuzzleFlash.Play();
        StartCoroutine(MuzzleLight());

        soundSource.PlayOneShot(shootClip, 0.5f);

        // Shoots a ray at the camera facing the same way as the camera and
        // travels as far as the range adnt he assigns the results to "hit"
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, Range))
        {

            // Checks to see if the object has the script then makes it take damage
            ShotScript targetShot = hit.transform.GetComponent<ShotScript>();
            if (targetShot != null)
            {
                targetShot.TakeDamage(Damage);
                GameObject Acid = Instantiate(AcidBlood, hit.point, Quaternion.LookRotation(hit.normal));
                Acid.transform.position += Acid.transform.forward / 100;
                Destroy(Acid, 0.5f);
            }
           else
            {
                // Creates a Bullet Hole at the point the object got shot and moves it forward to stop clipping and destorys it 2 seconds later
                GameObject hole = Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                BulletHole.transform.position += BulletHole.transform.forward / 100;
                Destroy(hole, 2f);
            }

            // Checks to see if the object has the script then makes it "Alerted"
            AiScript targetAi = hit.transform.GetComponent<AiScript>();
            if (targetAi != null) StartCoroutine(targetAi.AlertEnemy());

            // Applys a force in the dircetion the bullet came from 
            //if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * (Damage * 10));

        }
    }
}
