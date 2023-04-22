using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    public AudioSource[] titleSources;
    // Start is called before the first frame update
    void Start()
    {
        titleSources = GetComponents<AudioSource>();
        titleSources[0].Play();

    }
}

