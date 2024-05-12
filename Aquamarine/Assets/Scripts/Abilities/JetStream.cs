using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetStream : PlayerAbilities
{
    public override void UseAbility()
    {
        Debug.Log("Crashing Stream");
    }

    public override string ToString()
    {
        return "JetStream";
    }
}
