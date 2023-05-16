using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class AiScript : MonoBehaviour
{

    [Range(1, 360)]
    public float FOVangle;

    public float SightRange;
    public float SightMuliplier;
    public float AttackRange;
    public float AttackDamage;
    public float hitSpeed;
    public float WalkRange;

    public GameObject Player;
    public GameObject Spawner;
    public GameObject Acid;
    public Transform UILook;
    public NavMeshAgent agent;
    public LayerMask isPlayer, isWall;
    bool seeable, atDes;
    public bool isQueen;
    private bool isAttacking;

    GameManager GM;

    public Animator animationSource;

    float cooldown = 0;
    public float attackCooldown;
    public bool Attackable = false;
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
            StartCoroutine(Attacking());
            Attackable = true;
        }
        else
        {
            Attackable = false;
        }
    }

    public IEnumerator AlertEnemy()
    {
        float oldFOV = FOVangle;
        float oldSight = SightRange;
        float oldSpeed = agent.speed;

        agent.speed = hitSpeed;
        yield return new WaitForSeconds(0.5f);
        agent.speed = oldSpeed;

        FOVangle = 360;
        SightRange *= SightMuliplier;
        yield return new WaitForSeconds(5f);

        FOVangle = oldFOV;
        SightRange = oldSight;
    }

    private IEnumerator Attacking()
    {
        //agent.SetDestination(transform.position);
        if (attackCooldown < 0)
        {   
            isAttacking = true;
            float oldSpeed = agent.speed;
            agent.speed = oldSpeed * 2f;
            float oldRange = AttackRange;
            AttackRange *= 0.5f;
            StartCoroutine(GM.DamagePlayer(AttackDamage, this.gameObject));
            animationSource.SetTrigger("trAttack");
            attackCooldown = 6;
            mainSource.clip = attackClips[Random.Range(0, attackClips.Length)];
            mainSource.PlayDelayed(1);
            yield return new WaitForSeconds(1.5f);
            isAttacking = false;
            agent.speed = oldSpeed;
            AttackRange = oldRange;
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

        // If the AI is a Queen theres a 1% chance they'll spawn a Acid Pool 
        if (Random.Range(0, 100) == 0 && isQueen) AcidPool();

        // Plays a Random animation every 2 to 10 seconds
        //InvokeRepeating("IdleSFX", Random.Range(10,20), 1);
    }

    private void AcidPool()
    {
        this.gameObject.GetComponent<SpawnScript>().SpawnEnemies(Random.Range(1, 3), Acid);
    }
}
