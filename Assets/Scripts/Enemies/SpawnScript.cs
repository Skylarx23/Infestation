using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] float Range;
    public List<GameObject> Enemies;
    public GameObject Model;

    private void Update()
    {
        if (Input.GetKey(KeyCode.G)) SpawnEnemies(1, Model);
    }

    public void SpawnEnemies(int Amount, GameObject Enemy)
    {
        // Sets SpawnPoint to position of the object
        Vector3 SpawnPoint = transform.position;

        for (int i = 0; i < Amount; i++)
        {
            // Creates a offset (0.0 to 1.0) then times it by Range then halfs the range so we can have negative numbers too
            Vector3 offset = new Vector3(Random.value * Range - Range, 0, Random.value * Range - Range);

            Quaternion SpawnRotation = Enemy.transform.rotation;
            GameObject GOEnemy = Instantiate(Enemy, SpawnPoint + offset, SpawnRotation);
            GOEnemy.GetComponent<AiScript>().Spawner = this.gameObject;
            Enemies.Add(GOEnemy);
        }
    }
}
