using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public float Health = 50;
    public void TakeDamage(float Amount)
    {
        Health -= Amount;
        if (Health <= 0) Die();
    }

    private void Die()
    {
       Destroy(gameObject);
    }
}
