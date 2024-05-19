using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBedStatue : MonoBehaviour
{
    private bool touched = false;
    private bool abilityAdded = false;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touched)
        {
            player.SetPlayerInactive();
            if (dialogue != null)
            {
                StartCoroutine(BeginDialogue());
            }
            // Play Dialogue

        }

        touched = true;
        if (!PlayerPrefs.HasKey("WaterBed"))
        {
            PlayerPrefs.SetString("WaterBed", "true");
            PlayerPrefs.Save();
        }
        AddWaterBedToPlayerAbilities();
    }

    private void AddWaterBedToPlayerAbilities()
    {
        if (!abilityAdded)
        {
            PlayerAttribute attribute = PlayerAttribute.instance;
            attribute.AddWaterBedToAbilityList();
        }
        abilityAdded = true;
    }

    private IEnumerator BeginDialogue()
    {
        dialogue.StartDialogue();
        while(dialogue.started)
        {
            yield return null;
        }
        player._active = true;
    }

}
