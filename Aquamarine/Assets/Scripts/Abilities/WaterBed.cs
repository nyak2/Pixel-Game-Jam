using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBed : PlayerAbilities
{
    public override void UseAbility()
    {
        Debug.Log("Deploying Platform");
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        CreateWaterPlatform();
    }
    private void CreateWaterPlatform()
    {
        // Come back to this later to obtain water body coordinates
        // and spawn water platform above it
        PlayerAttribute.SpawnWaterPlatform();
    }

    public override string ToString()
    {
        return "Water Bed";
    }

}
