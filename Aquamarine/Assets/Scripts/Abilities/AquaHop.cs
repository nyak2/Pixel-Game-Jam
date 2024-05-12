using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AquaHop : PlayerAbilities
{
    public override void UseAbility()
    {
        Debug.Log("One With The Water");
        PlayerAttribute.ChangeAbilityUsageStatus(true);

    }

    public override string ToString()
    {
        return "AquaHop";
    }

}
