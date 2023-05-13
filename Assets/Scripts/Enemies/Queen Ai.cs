using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using Random = UnityEngine.Random;

public class QueenAi : MonoBehaviour
{

    [Range(1, 360)]
    public float FOVangle;

    public float SightRange;
    public float SightMuliplier;
    public float AttackRange;
    public float AttackDamage;
    public float SpeedMuliplier;
    public float WalkRange;

    public GameObject Player;
    public GameObject Spawner;
    public Transform UILook;
    public NavMeshAgent agent;
    public LayerMask isPlayer, isWall;
    bool seeable, atDes;

    bool AcidPoolsActive;
    public GameObject[] AcidPools;

    public GameObject AcidProjectile;
    public Transform AcidlunchPoint;
    public float ProjectileVel;

    GameManager GM;

    public Animator animationSource;

    float cooldown = 0;
    public float attackCooldown;
    public bool isInRange = false;
    float footstepCooldown = 0;

    public AudioSource mainSource;
    public AudioSource backgroundSource;
    public AudioClip[] idleClips;
    public AudioClip[] chaseClips;
    public AudioClip[] backgroundClips;
    public AudioClip[] attackClips;
    public AudioClip[] deathClips;

    public void Awake()
    {
        //animationSource = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("First Person Player");
        Idle();
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        cooldown -= Time.deltaTime;
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, SightRange, isPlayer);

        if (rangeChecks.Length != 0)
        {
            Transform Target = rangeChecks[0].transform;
            Vector3 DirToTarget = (Target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, DirToTarget) < FOVangle / 2)
            {
                float DisToTarget = Vector3.Distance(transform.position, Target.position);

                if (!Physics.Raycast(transform.position, DirToTarget, DisToTarget, isWall)) Chasing();
                else seeable = false;
            }
            else seeable = false;
        }
        else if (seeable) seeable = false;

        if (!seeable && cooldown < 0) 
        {
             Idle();
             cooldown = 2;
        }


        if (Physics.CheckSphere(transform.position, AttackRange, isPlayer))
        {
            Attacking();
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }

    public IEnumerator AlertEnemy()
    {
        float oldFOV = FOVangle;
        float oldSight = SightRange;
        float oldSpeed = agent.speed;

        agent.speed *= SpeedMuliplier;
        yield return new WaitForSeconds(0.5f);
        agent.speed = oldSpeed;

        FOVangle = 360;
        SightRange *= SightMuliplier;
        yield return new WaitForSeconds(5f);

        FOVangle = oldFOV;
        SightRange = oldSight;
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);
        if (attackCooldown < 0)
        {
            int RNDAttack = Random.Range(0, 4);
            if (RNDAttack == 0) Leap();
            else if (RNDAttack == 1) SpawnShatter();
            else if (RNDAttack == 2 && !AcidPoolsActive) StartCoroutine(ActivateAcidPools());
            else if (RNDAttack == 3) SpawnProjectile();
            else if (RNDAttack == 4) Slam();

            attackCooldown = 3;
        }
    }

    // agent.SetDestination(transform.position);
    // if (attackCooldown< 0)
    // {
    // StartCoroutine(GM.DamagePlayer(AttackDamage, this.gameObject));
    // animationSource.SetTrigger("trAttack");
    // attackCooldown = 3;
    // mainSource.clip = attackClips[Random.Range(0, attackClips.Length)];
    // mainSource.PlayDelayed(1);
    // }

private void Chasing()
    {
        footstepCooldown -= Time.deltaTime;
        agent.SetDestination(Player.transform.position);
        seeable = true;
        animationSource.SetTrigger("trChase");
        if(footstepCooldown < 0)
        {
        backgroundSource.clip = backgroundClips[0];
        backgroundSource.Play();
        footstepCooldown = 2;
        }
    }

    private void Idle()
    {
        if (atDes)
        {
            // Gives the enemy a random destination to go to whenever they cant see the player
            Vector3 offset = new Vector3(Random.Range(-WalkRange, WalkRange), transform.position.y, Random.Range(-WalkRange, WalkRange));
            agent.SetDestination(agent.transform.position + offset);
        }
        else if (agent.remainingDistance <= 1) atDes = true;
        else atDes = false;
        animationSource.SetTrigger("trWalk");
    }

    private void SpawnShatter()
    {
        int ProjectileNum = 3;
        if (Random.Range(0, 1) == 0) ProjectileNum = 5;

        const double Radius = 1f;
        float AngleStep = 360f / ProjectileNum;
        float Angle = 0;

        // Spawns the Projectile at a certian angle depending on the amount of projectiles
        for (int i = 0; i < ProjectileNum; i++)
        {
            // Direction calcutions
            float ProjDirXPos = (float)(transform.position.x + Math.Sin((Angle * Math.PI) / 180) * Radius);
            float ProjDirYPos = (float)(transform.position.x + Math.Cos((Angle * Math.PI) / 180) * Radius);
            
            Vector3 ProjVector = new Vector3(ProjDirXPos, ProjDirYPos, 0);
            Vector3 ProjMoveDir = (ProjVector = transform.position).normalized * ProjectileVel;
            
            GameObject Projectile = Instantiate(AcidProjectile, transform.position, Quaternion.identity);
            Projectile.GetComponent<Rigidbody>().velocity = new Vector3(ProjMoveDir.x, 0, ProjMoveDir.y);

            Angle += AngleStep;
        }
    }

    private IEnumerator ActivateAcidPools() 
    {
        // I dont know how to play the Roar

        AcidPoolsActive = true;
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(true);
        }
        yield return new WaitForSeconds(Random.Range(10, 15));  
        for (int i = 0; i < AcidPools.Length; i++)
        {
            AcidPools[i].SetActive(false);
        }
        AcidPoolsActive = false;
    }

    private void SpawnProjectile()
    {
        // I still dont know how to play the Roar
        GameObject Projectile = Instantiate(AcidProjectile, transform.position, Quaternion.identity);
        Projectile.GetComponent<Rigidbody>().velocity = AcidlunchPoint.up * ProjectileVel;
    }

    private void Leap()
    {

    }

    private void Slam()
    {
        
    }
}
