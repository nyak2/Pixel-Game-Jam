using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AquaHop : PlayerAbilities
{

    public override void UseAbility()
    {
        Player player = Player.instance;
        Debug.Log("One With The Water");
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        player.AquaTeleportation();
        PlayerAttribute.ChangeAbilityUsageStatus(false);
    }

    public override string ToString()
    {
        return "AquaHop";
    }

}
