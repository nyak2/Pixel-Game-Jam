using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStageDialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dialogue != null)
        {
            player._active = false;
            StartCoroutine(BeginDialogue());
        }
        // Play Dialogue
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
