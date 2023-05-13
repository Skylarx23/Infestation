using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class QueenAI : MonoBehaviour
{
    public float SpeedMuliplier;

    public GameObject Player;
    public GameObject Spawner;
    public Transform UILook;
    public NavMeshAgent agent;
    public LayerMask isPlayer, isWall;

    // Attacks
    public GameObject[] AcidPools;
    public GameObject earthShatter;
    public GameObject SlamPartical;
    public GameObject AcidProjectile;

    bool isAttacking;
    public float MeleeDamage;

    float footstepCooldown = 0;

    public Animator animationSource;
    public AudioSource mainSource;
    public AudioSource backgroundSource;
    public AudioClip[] chaseClips;
    public AudioClip[] backgroundClips;
    public AudioClip[] attackClips;
    public AudioClip deathClip;

    GameManager GM;

    public void Awake()
    {
        //animationSource = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("First Person Player");

        Debug.Log("Awake");
        StartCoroutine(Resting());
    }

    public IEnumerator AlertEnemy()
    {
        float oldSpeed = agent.speed;

        agent.speed *= SpeedMuliplier;
        yield return new WaitForSeconds(0.5f);
        agent.speed = oldSpeed;
    }

    private void Update()
    {
        float DistenceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        // If player is closer than 20 Units (?) it stops else it'll keep following the player
        if (DistenceToPlayer <= 20)
        {
            agent.SetDestination(transform.position);
            agent.transform.LookAt(new Vector3(Player.transform.position.x, 0, Player.transform.position.z));
        }
        else Chasing();

        // If the player gets too close the queen will attack
        if (DistenceToPlayer <= 8 && !isAttacking)
        {
            isAttacking = true;
            animationSource.SetTrigger("trAttack");
            GM.DamagePlayerHazard(MeleeDamage);
        }
    }

    public IEnumerator Resting()
    {
        Debug.Log("Resting");

        isAttacking = false;
        agent.speed = 11;

        // Every 5-10 seconds queen tries to attack
        yield return new WaitForSeconds(Random.Range(5, 10));
        Attacking();
    }

    private void Attacking()
    {

        if (!isAttacking)
        {
            isAttacking = true;

            int RNDAttack = Random.Range(0, 4);
            if (RNDAttack == 0) StartCoroutine(Leap());
            //else if (RNDAttack == 1) SpawnShatter();
            else if (RNDAttack == 2) StartCoroutine(ActivateAcidPools());
            else if (RNDAttack == 3) StartCoroutine(SpawnProjectile());
            else if (RNDAttack == 4) StartCoroutine(Slam());
        }
        else Resting();
    }

    private void Chasing()
    {
        footstepCooldown -= Time.deltaTime;
        agent.SetDestination(Player.transform.position);
        animationSource.SetTrigger("trChase");
        if (footstepCooldown < 0)
        {
            backgroundSource.clip = backgroundClips[0];
            backgroundSource.Play();
            footstepCooldown = 2;
        }
    }

    private void SpawnShatter()
    {

        

    }

    private IEnumerator ActivateAcidPools()
    {
        Debug.Log("acid pools");
        // I dont know how to play the Roar
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(true);
        }
        yield return new WaitForSeconds(Random.Range(10, 15));
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(false);
        }
        Resting();
    }

    private IEnumerator SpawnProjectile()
    {
        Debug.Log("acid projectile");
        agent.speed = 0;
        for (int i = 0; i < 2; i++)
        {
            Instantiate(AcidProjectile, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1);
        }
        Resting();
    }

    private IEnumerator Leap()
    {
        Debug.Log("Leap");

        agent.speed = 0;
        yield return new WaitForSeconds(2);
        agent.speed = 40;

        // Freezes the Queen for 0.5s and then plays the leap animation
        this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        yield return new WaitForSeconds(0.5f);
        animationSource.SetTrigger("trLeap");
        this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        Resting();
    }

    private IEnumerator Slam()
    {
        Debug.Log("Slam");
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        animationSource.SetTrigger("trSlam");
        Destroy(Instantiate(SlamPartical, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity), 3f);

        Resting();
    }

    public void QueenDeath()
    {

    }
}
