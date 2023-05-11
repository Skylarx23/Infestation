using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShotScript : MonoBehaviour
{
    public Slider HealthBar;
    //public Slider HealthBarGlobal;
    public float Health = 50;

    public GameObject AcidBlood;

    private void Start()
    {
        HealthBar.maxValue = Health;
        //HealthBarGlobal.maxValue = Health;
    }
    private void Update()
    {
        HealthBar.value = Health;
       // HealthBarGlobal.value = Health;
    }
    public void TakeDamage(float Amount)
    {
        Health -= Amount;
        if (Health <= 0) Die();
    }

    private void Die()
    {
        // Decreases Enemies Count from a spawner if they came from a spawner
        if (this.gameObject.GetComponent<AiScript>().Spawner != null)
        { 
        this.gameObject.GetComponent<AiScript>().Spawner.GetComponent<SpawnScript>().Enemies.Remove(this.gameObject);
        }

        // Creates a "AcidBlood" the destroy's itself and then the acid after 3 seconds
        Destroy(this.gameObject);
        GameObject Acid = Instantiate(AcidBlood, transform.position, Quaternion.LookRotation(transform.position));
        Destroy(Acid, 3f);
    }
}
