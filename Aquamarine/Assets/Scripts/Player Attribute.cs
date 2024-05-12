using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    // WaterBed, JetStream, HydroWard, AquaHop
    private List<PlayerAbilities> abilities = new List<PlayerAbilities>();
    public PlayerAbilities currentAbility;
    private static bool isAbilityBeingUsed = false;
    private int i = 0;
    private static GameObject waterPlatform;
    [SerializeField] private GameObject waterPlat;

    private void Start()
    {
        abilities.Add(new WaterBed());
        abilities.Add(new JetStream());
        abilities.Add(new HydroWard());
        abilities.Add(new AquaHop());
        currentAbility = abilities[0];
        waterPlatform = waterPlat;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAbility();
    }

    public void UpdateAbility()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAbilityBeingUsed)
            {
                currentAbility.UseAbility();

            }
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
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
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
        else
        {
            switch (Input.inputString)
            {
                case "1":
                    i = 0;
                    break;

                case "2":
                    i = 1;
                    break;

                case "3":
                    i = 2;
                    break;

                case "4":
                    i = 3;
                    break;
            }
        }
        currentAbility = abilities[i];
    }

    public static void ChangeAbilityUsageStatus(bool b)
    {
        isAbilityBeingUsed = b;
    }

    public async static void SpawnWaterPlatform()
    {
        GameObject tempPlatform = Instantiate(waterPlatform, new Vector3(0,0,0), Quaternion.identity);
        await Task.Delay(3000);
        Destroy(tempPlatform);
        ChangeAbilityUsageStatus(false);
       
    }

}
