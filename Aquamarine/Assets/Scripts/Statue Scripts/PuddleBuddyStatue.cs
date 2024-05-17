using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleBuddyStatue : MonoBehaviour
{
    private bool touched = false;
    private bool abilityAdded = false;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touched)
        {
            player._active = false;
            player.GetComponent<Animator>().Play("idle", 0, 0);
            if (dialogue != null)
            {
                StartCoroutine(BeginDialogue());
            }
            // Play Dialogue
        }

        touched = true;
        if (!PlayerPrefs.HasKey("PuddleBuddy"))
        {
            PlayerPrefs.SetString("PuddleBuddy", "true");
            PlayerPrefs.Save();
        }
        AddPuddleBuddyToPlayerAbilities();
    }

    private void AddPuddleBuddyToPlayerAbilities()
    {
        if (!abilityAdded)
        {
            PlayerAttribute attribute = PlayerAttribute.instance;
            attribute.AddPuddleBuddyToAbilityList();
        }
        abilityAdded = true;
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
