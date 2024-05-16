using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBedStatue : MonoBehaviour
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
}
