using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public float SightRange;
    public float SightMuliplier;

    [Range(0, 360)]
    public float FOVangle;

    public float AttackRange;
    public float AttackDamage;

    public float WalkRange;
    public LayerMask isPlayer, isWall;
    bool seeable, atDes;

    public GameObject Player;
    public NavMeshAgent agent;

    GameManager GM;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, SightRange, isPlayer);

        if (rangeChecks.Length != 0)
        {
            Transform Target = rangeChecks[0].transform;
            Vector3 DirToTarget = (Target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, DirToTarget) < FOVangle / 2)
            {
                float DisToTarget = Vector3.Distance(transform.position, Target.position);

                if (!Physics.Raycast(transform.position, DirToTarget, DisToTarget, isWall))
                {
                    agent.SetDestination(Player.transform.position);
                    seeable = true;
                }
                else seeable = false;
            }
            else seeable = false;
        }
        else if (seeable) seeable = false;

        if (!seeable)
        {
            if (atDes)
            {
                // Gives the enemy a random destination to go to whenever they cant see the player
                Vector3 offset = new Vector3(Random.Range(-WalkRange, WalkRange), 0, Random.Range(-WalkRange, WalkRange));
                agent.SetDestination(agent.transform.position + offset);
            }
            else if (agent.remainingDistance <= 1) atDes = true;
            else atDes = false;
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player")) GM.DamagePlayer(AttackDamage);
    }
}
