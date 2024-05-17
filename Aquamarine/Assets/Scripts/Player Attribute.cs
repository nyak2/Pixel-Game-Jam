using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttribute : MonoBehaviour
{
    public static PlayerAttribute instance;
    // WaterBed, AquaHop, HydroWard, PuddleBuddy
    private List<PlayerAbilities> abilities = new List<PlayerAbilities>();
    [SerializeField] public PlayerAbilities currentAbility { get; set; }
    private static bool isAbilityBeingUsed = false;
    private int i = 0;
    [SerializeField] float spawnOffSet = 1.32f;

    [SerializeField] private Transform waterCheck;
    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private LayerMask waterPlatformLayer;
    [SerializeField] private GameObject waterPlatform;
    [SerializeField] private GameObject waterSource;

    [SerializeField] private Animator playeranim;
    [SerializeField] private List<Sprite> abilitesSprites;
    [SerializeField] private Image abilityIcon;
    [SerializeField] private TextMeshProUGUI flashText;

    private void Start()
    {
        instance = this;
        abilityIcon.color = new Color(1f, 1f, 1f, 0f);
        if (PlayerPrefs.HasKey("WaterBed")) AddWaterBedToAbilityList();
        if (PlayerPrefs.HasKey("AquaHop")) AddAquaHopToAbilityList();
        if (PlayerPrefs.HasKey("HydroWard")) AddHydroWardToAbilityList();
        if (PlayerPrefs.HasKey("PuddleBuddy")) AddPuddleBuddyToAbilityList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAbility();
    }

    public void UpdateAbility()
    {
        if(playeranim.GetCurrentAnimatorStateInfo(0).IsName("ability"))
        {
            return;
        }

        if (IsOnWaterSource())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playeranim.Play("ability", 0, 0);
            }
        }

        if (abilities.Count > 0)
        {
            abilityIcon.color = new Color(1f, 1f, 1f, 1f);

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (i == abilities.Count - 1)
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
                if (i == 0)
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
                        if (abilities.Count >= 1)
                            i = 0;
                        break;

                    case "2":
                        if (abilities.Count >=2)
                            i = 1;
                        break;

                    case "3":
                        if (abilities.Count >= 3)
                            i = 2;
                        break;

                    case "4":
                        if (abilities.Count >= 4)
                            i = 3;
                        break;
                }
            }
            currentAbility = abilities[i];
            abilityIcon.sprite = abilitesSprites[i];
        }
    }

    public void AddPuddleBuddyToAbilityList()
    {
        abilities.Add(new PuddleBuddy());
    }

    public void AddHydroWardToAbilityList()
    {
        abilities.Add(new HydroWard());
    }

    public void AddWaterBedToAbilityList()
    {
        abilities.Add(new WaterBed());

        // Must Move If WaterBed is No Longer the First Ability.
        currentAbility = abilities[0];
    }

    public void AddAquaHopToAbilityList()
    {
        abilities.Add(new AquaHop());
    }

    public bool IsCurrAbilityAquaHop()
    {
        if (currentAbility == null) return false;
        return currentAbility.GetType() == typeof(AquaHop);
    }

    public bool IsCurrAbilityPuddleBuddy()
    {
        if (currentAbility == null) return false;
        return currentAbility.GetType() == typeof(PuddleBuddy);
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
        flashText.gameObject.SetActive(false);
        Thread.Sleep(100);
        ChangeAbilityUsageStatus(false);
    }

    public bool IsOnWaterSource()
    {
        return Physics2D.OverlapCircle(waterCheck.position, 0.2f, waterLayer);
    }

    public bool IsOnWaterPlatform()
    {
        return Physics2D.OverlapCircle(waterCheck.position, 0.2f, waterPlatformLayer);
    }

    public static void ChangeAbilityUsageStatus(bool b)
    {
        isAbilityBeingUsed = b;
    }

    public async void SpawnWaterSource()
    {
        ClearCurrentAbilityObject("Movable Water Source(Clone)");
        Vector2 spawnPos = ObtainSpawnPosition();
        
        GameObject tempWaterSource = Instantiate(waterSource, new Vector3(spawnPos.x,spawnPos.y - spawnOffSet,0), Quaternion.identity);
        ChangeAbilityUsageStatus(false);
        await Task.Delay(10000);
        Destroy(tempWaterSource);

    }

    public async void SpawnWaterPlatform()
    {
        ClearCurrentAbilityObject("Water Platform(Clone)");
        Vector2 spawnPos = ObtainSpawnPosition();

        GameObject tempPlatform = Instantiate(waterPlatform, new Vector3(spawnPos.x, spawnPos.y, 0), Quaternion.identity);
        flashText.gameObject.SetActive(true);
        await Task.Delay(3000);
        Destroy(tempPlatform);
        flashText.gameObject.SetActive(false);
        ChangeAbilityUsageStatus(false);
        
    }

    private Vector2 ObtainSpawnPosition()
    {
        return new Vector2(waterCheck.position.x, waterCheck.position.y + spawnOffSet);
    }

    public void EnableAbility()
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
