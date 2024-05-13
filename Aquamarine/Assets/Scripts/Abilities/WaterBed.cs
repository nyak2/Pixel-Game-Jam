using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBed : PlayerAbilities
{
    public override void UseAbility()
    {
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        CreateWaterPlatform();
    }

    private void CreateWaterPlatform()
    {
        PlayerAttribute.SpawnWaterPlatform();
    }

    public override string ToString()
    {
        return "Water Bed";
    }

}
