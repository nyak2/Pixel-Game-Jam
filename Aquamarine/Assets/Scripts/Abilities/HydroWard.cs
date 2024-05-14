using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Protects the Player from Dying for a Few Seconds, But Falling Outside the Map Causes Instant Death.
public class HydroWard : PlayerAbilities
{

    public override void UseAbility()
    {
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        _ = SpawnShield();
    }

    private async Task SpawnShield()
    {
        Player player = Player.instance;
        player.MakeProtected();
        await player.SlowMovementFor(2000);
        player.MakeUnProtected();
        PlayerAttribute.ChangeAbilityUsageStatus(false);
    }

}
