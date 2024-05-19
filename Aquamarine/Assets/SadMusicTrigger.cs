using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadMusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip sadMusic;
    [SerializeField] private AudioSource evilBgMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        evilBgMusic.clip = sadMusic;
        evilBgMusic.Play();
    }
}
