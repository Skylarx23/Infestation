using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    GameManager GM;

    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnTriggerEnter(Collider Collider)
    {
        if (Collider.gameObject.CompareTag("Player")) GM.CollectMedKit(this.gameObject);
    }
}
