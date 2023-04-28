using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public NavMeshAgent agent;
    Transform Player;
    Vector3 Destination;
    bool atPos;

    [SerializeField] float SightRange;
    public LayerMask isPlayer, isGround;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("First Person Player").transform;
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, SightRange, isPlayer))
        {
            // When near the play it'll move and look towards them
            transform.LookAt(transform.position + Player.transform.rotation * Vector3.forward, Player.transform.rotation * Vector3.up);
            agent.SetDestination(Player.position);
            atPos = true;
        }
        else
        {
            if (atPos)
            {
                // Picks around destination to go to
                Vector3 Offset = new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5));
                Destination = transform.position + Offset;
                agent.SetDestination(Destination);

                // If destination isnt valid it makes another
                if (Physics.Raycast(Destination, -transform.up, 2f, isGround)) atPos = false;
            }

            Vector3 DisToDest = transform.position - Destination;
            if (DisToDest.magnitude < 1) atPos = true;
        }


    }
}
