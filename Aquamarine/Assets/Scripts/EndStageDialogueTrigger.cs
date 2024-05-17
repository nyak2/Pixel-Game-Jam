using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStageDialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dialogue != null)
        {
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
    }
}
