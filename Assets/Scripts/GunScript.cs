using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float Range = 100;
    public float Damage = 10;
    public float FireRate = 10;
    private float NextTimetoFire = 0;

    public Camera PlayerCam;
    public ParticleSystem MuzzleFlash;
    public Light Flash;
    public GameObject BulletHole;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= NextTimetoFire)
        {
            // Slows fire rate to 1/Firerate, so if FireRate = 10 then you can only fire every 0.1s
            NextTimetoFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }

    private IEnumerator MuzzleLight()
    {
        Flash.enabled = true;
        yield return new WaitForSeconds(0.01f);
        Flash.enabled = false;
    }

    void Shoot()
    {
        RaycastHit hit;

        // Plays partical system and shines light
        MuzzleFlash.Play();
        StartCoroutine(MuzzleLight());

        // Shoots a ray at the camera facing the same way as the camera and
        // travels as far as the range adnt he assigns the results to "hit"
        if (Physics.Raycast(PlayerCam.transform.position, PlayerCam.transform.forward, out hit, Range))
        {
            // Creates a Bullet Hole at the point the object got shot and moves it forward to stop clipping adn destorys it 2 seconds later
            GameObject hole = Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            BulletHole.transform.position += BulletHole.transform.forward / 100;
            Destroy(hole, 2f);

            // Checks to see if the object has the script then makes it take damage
            ShotScript target = hit.transform.GetComponent<ShotScript>();
            if (target != null) target.TakeDamage(Damage);

            // Applys a force in the dircetion the bullet came from 
            if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * (Damage * 10));

        }
    }
}
