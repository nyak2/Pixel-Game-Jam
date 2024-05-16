using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroWardStatue : MonoBehaviour
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
        if (!PlayerPrefs.HasKey("HydroWard"))
        {
            PlayerPrefs.SetString("HydroWard", "true");
            PlayerPrefs.Save();
        }
        AddHydroWardToPlayerAbilities();
    }

    private void AddHydroWardToPlayerAbilities()
    {
        if (!abilityAdded)
        {
            PlayerAttribute attribute = PlayerAttribute.instance;
            attribute.AddHydroWardToAbilityList();
        }
        abilityAdded = true;
    }
}
