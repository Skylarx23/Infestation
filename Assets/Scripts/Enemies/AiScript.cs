using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public float SightRange;
    public float SightMuliplier;

    [Range(1, 360)]
    public float FOVangle;

    public float AttackRange;
    public float AttackDamage;

    public float WalkRange;
    public LayerMask isPlayer, isWall;
    bool seeable, atDes;

    public GameObject Player;
    public NavMeshAgent agent;

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

        StartCoroutine(GM.DamagePlayer(AttackDamage));
        animationSource.SetTrigger("trAttack");
        attackCooldown = 3;
        mainSource.clip = attackClips[Random.Range(0, attackClips.Length)];
        mainSource.PlayDelayed(1);
        }
    }

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
        Debug.Log("footstep");
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

        // Plays a Random animation every 2 to 10 seconds
        InvokeRepeating("IdleSFX", Random.Range(2, 10), 1);
    }

    public void IdleSFX()
    {
        mainSource.clip = chaseClips[Random.Range(0, chaseClips.Length)];
        mainSource.PlayDelayed(1);
    }


    public void AlienDie()
    {
        Debug.Log("dead");

    }
}
