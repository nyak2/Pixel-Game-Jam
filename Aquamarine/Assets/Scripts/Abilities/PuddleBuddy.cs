using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleBuddy : PlayerAbilities
{
    public override void UseAbility()
    {
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        PlayerAttribute.instance.SpawnWaterSource();
    }

}
