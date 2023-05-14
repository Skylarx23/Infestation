using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollider : MonoBehaviour
{
    GameManager GM;
    public float damageAmount;
    // Start is called before the first frame update

    private void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        GM.DamagePlayerHazard(damageAmount);
    }

}
