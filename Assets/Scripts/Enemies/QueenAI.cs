using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class QueenAI : MonoBehaviour
{
    public float Health;
    public GameObject Player;
    public GameObject Spawner;
    public Transform UILook;
    public NavMeshAgent agent;
    public LayerMask isPlayer, isWall;
    public float MeleeRange;
    private bool isLeaping;
    private bool acidAttackInProgess = false;
    private bool isPhase2 = false;
    private bool isPhase3 = false;
    private bool hasPlayed = false;

    // Attacks
    public GameObject[] AcidPools;
    public GameObject earthShatter;
    public GameObject earthShatterLong;
    public GameObject shatterSpawner;
    public GameObject SlamPartical;
    public GameObject AcidProjectile;
    public GameObject acidSpawner;

    bool isAttacking;
    public float MeleeDamage;
    private bool enemiesSpawned = false;
    private bool isDead = false;
    private float rotationCooldown = 0;

    float footstepCooldown = 0;
    public float meleeCooldown = 0;

    public Animator animationSource;
    public AudioSource mainSource;
    public AudioSource backgroundSource;
    public AudioClip[] chaseClips;
    public AudioClip[] backgroundClips;
    public AudioClip[] attackClips;
    public AudioClip[] roarClips;
    public AudioClip deathClip;
    public AudioClip shatterClip;
    public AudioClip projectileClip;

    GameManager GM;

    public void Awake()
    {
        //animationSource = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("First Person Player");
        StartCoroutine(Resting());
    }

    public void Update()
    {
        Health = this.GetComponent<ShotScript>().Health;
        GM.QueenHealth = Health;
        if(Health < 40000 && hasPlayed == false)
        {
            mainSource.clip = roarClips[Random.Range(0, roarClips.Length)];
            GM.QueenPhase2();
            isPhase2 = true;
            hasPlayed = true;
            StartCoroutine(GM.QueenSpawns());
        }
        //if(Health < 20000 && isPhase3 == false)
        //{
            //isPhase2 = false;
            //sPhase3 = true;
            //StartCoroutine(GM.QueenSpawns());
        //}
        if(Health < 0 && isDead == false)
        {
            QueenDeath();
        }

        float DistenceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        meleeCooldown -= Time.deltaTime;
        rotationCooldown -= Time.deltaTime;
        animationSource.SetTrigger("trChase");

        if(rotationCooldown <= 0 && !isLeaping)
        {
            agent.SetDestination(Player.transform.position);
            agent.transform.LookAt(new Vector3(Player.transform.position.x, 0, Player.transform.position.z));
        }

        if (DistenceToPlayer >= 50 && !isAttacking && !isLeaping)
        {
            agent.speed = 25;
        }
        

        if (Physics.CheckSphere(transform.position, MeleeRange, isPlayer) && !isAttacking)
        {
            if (meleeCooldown < 0)
            {
                animationSource.SetTrigger("trAttack");
                Debug.Log("QueenMelee");
                StartCoroutine(GM.DamagePlayerHazard(400));
                meleeCooldown = 3;
                mainSource.clip = attackClips[Random.Range(0, attackClips.Length)];
                mainSource.PlayDelayed(1);
            }
        }
        // If player is closer than 20 Units (?) it stops else it'll keep following the player
    }

    public IEnumerator Resting()
    {
        Debug.Log("Resting");
        isAttacking = false;
        agent.speed = 15;

        // Every 5-10 seconds queen tries to attack
        if(isPhase2 == false && isPhase3 == false)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            Attacking();
        }
        else if(isPhase2 == true)
        {
            yield return new WaitForSeconds(Random.Range(4, 8));
            Attacking();
        }
        //else if(isPhase3 == true)
        //{
           // Debug.Log("phase 3 attack");
            //yield return new WaitForSeconds(Random.Range(2, 4));
           // Attacking();
        //}
    }

    private void Attacking()
    {

        if (!isAttacking)
        {
            isAttacking = true;

            int RNDAttack = Random.Range(0, 4);
            if (RNDAttack == 0) StartCoroutine(Leap());
            else if (RNDAttack == 1) StartCoroutine(SpawnShatter());
            else if (RNDAttack == 2) StartCoroutine(SpawnProjectile());
            else if (RNDAttack == 3) StartCoroutine(Slam());
        }
        else StartCoroutine(Resting());
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

    private IEnumerator SpawnShatter()
    {
        float oldSpeed = agent.speed;
        for (int i = 0; i < 3; i++)
        {
            agent.speed = 0;
            mainSource.clip = roarClips[Random.Range(0, roarClips.Length)];
            mainSource.PlayDelayed(1);
            animationSource.SetTrigger("trShatter");
            yield return new WaitForSeconds(0.75f);
            backgroundSource.PlayOneShot(shatterClip, 0.8f);
            float DistenceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (DistenceToPlayer <= 30)
            {
                Destroy(Instantiate(earthShatter, shatterSpawner.transform.position, shatterSpawner.transform.rotation), 3f);
            }
            else if(DistenceToPlayer >= 30)
            {
                Debug.Log("shatterlong");
                Destroy(Instantiate(earthShatterLong, shatterSpawner.transform.position, shatterSpawner.transform.rotation), 3f);
            }
            yield return new WaitForSeconds(1.5f);
            agent.speed = oldSpeed;
            agent.SetDestination(Player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false;
        StartCoroutine(Resting());
    }

    private IEnumerator ActivateAcidPools()
    {
        Debug.Log("acid pools");
        // I dont know how to play the Roar
        acidAttackInProgess = true;
        mainSource.clip = roarClips[Random.Range(0, roarClips.Length)];
        mainSource.PlayDelayed(1);
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(true);
        }
        isAttacking = false;
        StartCoroutine(Resting());
        yield return new WaitForSeconds(Random.Range(10, 15));
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(false);
        }
        acidAttackInProgess = false;
    }

    private IEnumerator SpawnProjectile()
    {
        float oldSpeed = agent.speed;
        agent.speed = 6;
        for(int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(0.25f);
            GameObject acid = (GameObject)Instantiate(AcidProjectile, acidSpawner.transform.position, acidSpawner.transform.rotation, acidSpawner.transform);
            backgroundSource.PlayOneShot(projectileClip, 0.9f);
            yield return new WaitForSeconds(2f);
            Destroy(acid);
        }
        agent.speed = oldSpeed;
        isAttacking = false;
        StartCoroutine(Resting());
    }

    private IEnumerator Leap()
    {   
        float oldDistance = agent.stoppingDistance;
        agent.stoppingDistance = 0;
        float oldSpeed = agent.speed;
        agent.speed = 1;
        isLeaping = true;
        animationSource.SetTrigger("trLeap");
        mainSource.clip = roarClips[Random.Range(0, roarClips.Length)];
        mainSource.PlayDelayed(1);
        yield return new WaitForSeconds(0.5f);

        // Freezes the Queen for 0.5s and then plays the leap animation
        //this.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        //yield return new WaitForSeconds(0.5f);
        agent.speed = 400;
        agent.SetDestination(Player.transform.position);
        yield return new WaitForSeconds(2f);
        agent.SetDestination(Player.transform.position);
        //this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
        isLeaping = false;
        agent.speed = oldSpeed;
        isAttacking = false;
        agent.stoppingDistance = oldDistance;
        StartCoroutine(Resting());
    }

    private IEnumerator Slam()
    {
        agent.speed = 0;
        yield return new WaitForSeconds(0.5f);
        mainSource.clip = roarClips[Random.Range(0, roarClips.Length)];
        mainSource.PlayDelayed(1);
        animationSource.SetTrigger("trSlam");
        yield return new WaitForSeconds(1.5f);
        Destroy(Instantiate(SlamPartical, shatterSpawner.transform.position, acidSpawner.transform.rotation, acidSpawner.transform), 0.5f);
        backgroundSource.PlayOneShot(shatterClip, 0.8f);
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        StartCoroutine(Resting());
    }
    
    public void QueenDeath()
    {
        StartCoroutine(GM.QueenDeath());
        isDead = true;
    }
}
