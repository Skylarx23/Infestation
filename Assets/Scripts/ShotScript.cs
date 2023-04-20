using UnityEngine;
using UnityEngine.UI;

public class ShotScript : MonoBehaviour
{
    public Slider HealthBar;
    public float Health = 50;

    private void Start()
    {
        HealthBar.maxValue = Health;
    }
    private void Update()
    {
        HealthBar.value = Health;
    }
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
