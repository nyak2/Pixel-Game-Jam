using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaHopStatue : MonoBehaviour
{
    private bool touched = false;
    private bool abilityAdded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touched)
        {
            Player player = Player.instance;
            player._active = false;
            // Play Dialogue

            player._active = true;
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
}
