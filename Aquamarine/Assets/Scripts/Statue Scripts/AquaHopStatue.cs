using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaHopStatue : MonoBehaviour
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
        }

        touched = true;
        if (!PlayerPrefs.HasKey("AquaHop"))
        {
            PlayerPrefs.SetString("AquaHop", "true");
            PlayerPrefs.Save();
        }
        AddAquaHopToPlayerAbilities();
    }

    private void AddAquaHopToPlayerAbilities()
    {
        if (!abilityAdded)
        {
            PlayerAttribute attribute = PlayerAttribute.instance;
            attribute.AddAquaHopToAbilityList();
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
