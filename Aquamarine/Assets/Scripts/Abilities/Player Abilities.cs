using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    // WaterBed, JetStream, HydroWard, AquaHop
    private List<PlayerAbilities> abilities = new List<PlayerAbilities>();
    private PlayerAbilities currentAbility;
    private int i = 0;

    private void Start()
    {
        abilities.Add(new WaterBed());
        abilities.Add(new JetStream());
        abilities.Add(new HydroWard());
        abilities.Add(new AquaHop());
        currentAbility = abilities[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility(currentAbility);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentAbility.GetType() == typeof(AquaHop))
            {
                i = 0;
            } 
            else
            {
                i += 1;
            }
        }
        else
        {
            if (currentAbility.GetType() == typeof(WaterBed))
            {
                i = abilities.Count - 1;
            }
            else
            {
                i -= 1;
            }
        }
        currentAbility = abilities[i];
    }

    protected virtual void UseAbility(PlayerAbilities ability) 
    {

    }
}
