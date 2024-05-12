using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroWard : PlayerAbilities
{
    public override void UseAbility()
    {
        Debug.Log("Shield Going Out");
    }

    public override string ToString()
    {
        return "Hydro Ward";
    }

}
