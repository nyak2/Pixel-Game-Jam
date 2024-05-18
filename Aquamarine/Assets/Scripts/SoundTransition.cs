using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTransition : MonoBehaviour 
{
    public AudioSource bgMusic;
    public AudioClip instenseBgMusic;
    private bool once;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!once)
        {
            bgMusic.clip = instenseBgMusic;
            bgMusic.Play();
            once = true;
        }
    }
}
