using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerAttribute : MonoBehaviour
{
    public static PlayerAttribute instance;
    // WaterBed, AquaHop, HydroWard, PuddleBuddy
    private List<PlayerAbilities> abilities = new List<PlayerAbilities>();
    public PlayerAbilities currentAbility { get; set; }
    private static bool isAbilityBeingUsed = false;
    private int i = 0;

    [SerializeField] private Transform waterCheck;
    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private GameObject waterPlatform;
    [SerializeField] private GameObject waterSource;

    private void Start()
    {
        instance = this;
        abilities.Add(new WaterBed());
        abilities.Add(new AquaHop());
        abilities.Add(new HydroWard());
        abilities.Add(new PuddleBuddy());
        currentAbility = abilities[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAbility();
    }

    public void UpdateAbility()
    {
        if (IsOnWaterSource())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isAbilityBeingUsed)
                {
                    currentAbility.UseAbility();
                }
                else
                {
                    // Clears All Abilities if Use Ability is Pressed While An Ability is Being Used.
                    ClearAllAbilities();
                }
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

    public bool IsCurrAbilityAquaHop()
    {
        return currentAbility == abilities[3];
    }

    public bool IsCurrAbilityJetStream()
    {
        return currentAbility == abilities[1];
    }

    public static void ClearCurrentAbilityObject(string abilityObject)
    {
        GameObject presentWaterBlock = GameObject.Find(abilityObject);
        if (presentWaterBlock != null) Destroy(presentWaterBlock);
    }

    public void ClearAllAbilities()
    {
        GameObject[] ablityObjects = GameObject.FindGameObjectsWithTag("Ability Object");
        foreach (GameObject obj in ablityObjects)
        {
            Destroy(obj);
        }
        Player.instance.MakeUnProtected();
        ChangeAbilityUsageStatus(false);
    }

    public bool IsOnWaterSource()
    {
        return Physics2D.OverlapCircle(waterCheck.position, 0.2f, waterLayer);
    }

    public static void ChangeAbilityUsageStatus(bool b)
    {
        isAbilityBeingUsed = b;
    }

    public void SpawnWaterSource()
    {
        ClearCurrentAbilityObject("Movable Water Source(Clone)");
        Vector2 spawnPos = ObtainSpawnPosition();
        
        Instantiate(waterSource, new Vector3(spawnPos.x,spawnPos.y - 2f,0), Quaternion.identity);
        ChangeAbilityUsageStatus(false);
       
    }

    public async void SpawnWaterPlatform()
    {
        ClearCurrentAbilityObject("Water Platform(Clone)");
        Vector2 spawnPos = ObtainSpawnPosition();

        GameObject tempPlatform = Instantiate(waterPlatform, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
        await Task.Delay(3000);
        Destroy(tempPlatform);
        ChangeAbilityUsageStatus(false);
        
    }

    private Vector2 ObtainSpawnPosition()
    {
        return new Vector2(waterCheck.position.x, waterCheck.position.y + 2f);
    }

}
