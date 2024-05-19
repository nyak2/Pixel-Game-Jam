using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2StartDialogue : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Dialogue dialogue;
    private bool touched = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touched)
        {
            player.SetPlayerInactive();
            if (dialogue != null)
            {
                StartCoroutine(BeginDialogue());
            }
            touched = true;
        }
        
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
