using UnityEngine;
using UnityEngine.AI;

public class AiScript : MonoBehaviour
{
    public float FOVangle;
    [SerializeField] float SightRange; [Range(0, 360)]
    public LayerMask isPlayer, isWall;
    bool seeable;
    GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("First Person Player");
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

                if (!Physics.Raycast(transform.position, DirToTarget, DisToTarget, isWall)) seeable = true;
                else seeable = false;
            }
            else seeable = false;
        }
        else if (seeable) seeable = false;
    }
}
