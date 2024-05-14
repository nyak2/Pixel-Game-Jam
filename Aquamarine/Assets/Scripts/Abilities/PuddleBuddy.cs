using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns A Temporary Movable Water Block that Behaves Exactly Like a Water Source.
public class PuddleBuddy : PlayerAbilities
{
    public override void UseAbility()
    {
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        PlayerAttribute.instance.SpawnWaterSource();
    }

}
