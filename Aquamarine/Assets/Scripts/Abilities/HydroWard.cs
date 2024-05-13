using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HydroWard : PlayerAbilities
{

    public override void UseAbility()
    {
        Debug.Log("Shield Going Out");
        PlayerAttribute.ChangeAbilityUsageStatus(true);
        _ = SpawnShield();
    }

    private async Task SpawnShield()
    {
        Player player = Player.instance;
        player.MakeProtected();
        await player.SlowMovementFor(3000);
        player.MakeUnProtected();
        PlayerAttribute.ChangeAbilityUsageStatus(false);
    }

    public override string ToString()
    {
        return "Hydro Ward";
    }

}
