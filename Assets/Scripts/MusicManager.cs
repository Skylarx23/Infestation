using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource soundSource;
    public AudioClip queenFightMusic;
    public AudioClip idleMusic1;
    public AudioClip fightTheme;
    public AudioClip ambience1, ambience2;

    // Start is called before the first frame update
    void Start()
    {
        soundSource.PlayOneShot(idleMusic1, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
