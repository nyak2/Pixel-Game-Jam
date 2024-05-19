using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Sets the Player's Respawn Point to the Shrine.
public class Shrine : MonoBehaviour
{
    private bool saved = false;
    [SerializeField] private GameObject savetext;
    [SerializeField] private AudioSource checkpointSfx;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!saved)
        {          
            checkpointSfx.Play();
            player.SetRespawnPoint(player.transform.position);
            saved = true;
            if (dialogue != null)
            {
                player.SetPlayerInactive();
                StartCoroutine(BeginDialogue());           
            }

            StartCoroutine(ShowSaveText());          
        }
        
    }

    private IEnumerator ShowSaveText()
    {
        Vector3 tempos = savetext.transform.position;
        LeanTween.moveLocalX(savetext, 0 , 0.5f).setEaseOutBack();
        yield return new WaitForSeconds(2.5f);
        LeanTween.moveLocalX(savetext, -1367 , 0.5f).setEaseInBack();
        yield return new WaitForSeconds(1.0f);
        savetext.transform.position = tempos;
    }

    private IEnumerator BeginDialogue()
    {
        dialogue.StartDialogue();
        while (dialogue.started)
        {
            yield return null;
        }
        player._active = true;
    }

}
