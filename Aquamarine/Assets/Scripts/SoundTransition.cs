using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTransition : MonoBehaviour 
{
    public AudioSource bgMusic;
    public AudioClip instenseBgMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bgMusic.clip = instenseBgMusic;
    }
}
