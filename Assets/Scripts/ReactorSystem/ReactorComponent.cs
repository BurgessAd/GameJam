﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorComponent : MonoBehaviour
{
    private SharedProperties attackDelay;
    private SharedProperties movementSpeed;
    private SharedProperties turretRotateSpeed;
    private SharedProperties acceleration;
    private SharedProperties treadsRotationSpeed;

    public GameObject powerBar;
    public GameObject moderatorBar;
    public float maxPower = 100;
    public float maxModerator = 100;
    public float maxHealth = 100;
    public float power = 100;
    public float moderator = 100;
    [Range(0, 1)]
    public float spinSpeedModifier;

    public ItemObject carbon;
    public ItemObject uranium;
    public ItemObject steel;


    public GameObject spinner;
    public GameObject spinningRods;

    enum PowerRegime
    {
        Extreme = 3, High = 2, Medium = 1, Low = 0
    }

    private PowerRegime currentPower;

    public float baseScale;

    private List<GameObject> robots = new List<GameObject>();

    [SerializeField]
    int numBotsToSpawn = 1;

    [SerializeField]
    GameObject botPrefab;

    [SerializeField]
    [Range(0.1f, 2f)]
    private float MaxAttackDelay = 1.0f;
    [SerializeField]
    [Range(1f, 4f)]
    private float MaxMovementSpeed = 3.0f;
    [SerializeField]
    [Range(1f, 4f)]
    private float MaxTurretRotate = 3.0f;
    [SerializeField]
    [Range(0.01f, 0.5f)]
    private float MaxAcceleration = 0.5f;
    [SerializeField]
    [Range(1f, 4f)]
    private float MaxTreadsRotationSpeed = 3.0f;
    [SerializeField]
    bool changedValues;

    [SerializeField]
    private AudioClip powerUpClip;

    [SerializeField]
    private AudioClip powerDownClip;



    private AudioSource audioSource;

    [SerializeField]
    private float[] pitcher = new float[] { 0.8f, 0.85f, 0.9f, 1.0f };


    public bool isPlayer = false;

    [SerializeField]
    bool spawnPlayer;

    [SerializeField]
    private float[] attackDelays = new float[4];
    [SerializeField]
    private float[] moveSpeeds=new float[4];
    [SerializeField]
    private float[] accelerations = new float[4];
    [SerializeField]
    private float[] treadsRotations = new float[4];
    [SerializeField]
    private float[] turretRotateSpeeds = new float[4];

    void PlayPowerUp()
    {
        audioSource.clip = powerUpClip;
        audioSource.pitch = pitcher[Mathf.Clamp((int)currentPower, 0, 3)];
        audioSource.Play();
    }

    void PlayPowerDown()
    {
        audioSource.clip = powerDownClip;
        audioSource.pitch = pitcher[Mathf.Clamp((int)currentPower, 0, 3)];
        audioSource.Play();
    }

    void CheckPowerRegimeChange()
    {
        int newPower = Mathf.FloorToInt(4 * power / maxPower);
        if (newPower< (int)currentPower)
        {
            currentPower = (PowerRegime)newPower;
            PlayPowerDown();
        }
        else if (newPower > (int)currentPower)
        {
            currentPower = (PowerRegime)newPower;
            PlayPowerUp();
        }
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        attackDelay = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        movementSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        turretRotateSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        acceleration = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        treadsRotationSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        changedValues = true;
        OnValidate();
        baseScale = powerBar.transform.localScale.x;
        powerBar.transform.localScale = Vector3.zero;
        moderatorBar.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        for (int i = 0; i < numBotsToSpawn; i++)
        {
            SpawnBot();
        }
        GetComponent<TeamComponent>().SetPlayer(isPlayer);
    }

    void SetPowerLevel(int powerPoint)
    {
        attackDelay.Value = attackDelays[powerPoint];
        movementSpeed.Value = moveSpeeds[powerPoint];
        acceleration.Value = accelerations[powerPoint];
        treadsRotationSpeed.Value = treadsRotations[powerPoint];
        turretRotateSpeed.Value = turretRotateSpeeds[powerPoint];
        CheckPowerRegimeChange();
    }

    void Update()
    {
        spinner.transform.Rotate(0, 0, power * spinSpeedModifier);
        spinningRods.transform.Rotate(0, 0, -power * spinSpeedModifier);
        if (isPlayer)
        {
            if (Input.GetKey(KeyCode.Space)&&(TerrainGenerator.player.transform.position-gameObject.transform.position).magnitude< 7)
            {
                replenishResources();
            }
            SetPowerLevel(Mathf.Clamp(Mathf.FloorToInt(4 * power/maxPower), 0, 3));
            depleteResources();
            powerBar.transform.localScale = new Vector3( baseScale*power / maxPower, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
            moderatorBar.transform.localScale = new Vector3( baseScale*moderator / maxModerator, moderatorBar.transform.localScale.y, moderatorBar.transform.localScale.z);
            if (robots.Count == 0||power<=0||moderator<=0)
            {
                GetComponent<HealthComponent>().Die();
                StartCoroutine(GameOver());
                
            }
        }
    }

    public void replenishResources()
    {
        int uraniumCount = 0;
        int carbonCount = 0;
        int steelCount = 0;
        InventoryComponent inventory = TerrainGenerator.player.GetComponent<InventoryComponent>();
        for (int i = 0; i <inventory.Container.Count;i++)
        {
            if (inventory.Container[i].item == steel)
            {
                steelCount = inventory.Container[i].currentAmount;
            }

            else if (inventory.Container[i].item == uranium)
            {
                uraniumCount = inventory.Container[i].currentAmount;
            }
            else if(inventory.Container[i].item == carbon)
            {
                carbonCount = inventory.Container[i].currentAmount;
            }
        }

        if (steelCount >= 20)
        {
            SpawnBot();
            inventory.AddItem(steel, -20);
        }


        if(power + 10*uraniumCount <= maxPower)
        {
            inventory.AddItem(uranium,-uraniumCount);
            power += 10*uraniumCount;
            
        }
        else
        {
            inventory.AddItem(uranium,-(int)(maxPower - power)/10);
            power = maxPower;
        }
        if (moderator + 10*carbonCount < maxModerator)
        {
            inventory.AddItem(carbon, -carbonCount);
            moderator += 10*carbonCount;
        }
        else
        {
            inventory.AddItem(carbon, -(int)(maxModerator- moderator)/10);
            moderator= maxModerator;
        }
        
    }


    public void killMe(GameObject _robot)
    {
        robots.Remove(_robot);
        if (_robot == TerrainGenerator.player)
        {
            int index = (int)Random.Range(0, robots.Count - 1);
            robots[index].GetComponent<MoveAuthorityComponent>().SetAuthority(GetComponent<PlayerInputComponent>());
            robots[index].AddComponent<CameraLookComponent>().ChangeCameraFocus(robots[index]);
            robots[index].name = "player";
            GetComponent<PlayerInputComponent>().ChangeControlledObject(robots[index]);
            TerrainGenerator.player = robots[index]; 
        }
        Destroy(_robot,0.2f);
    }



    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        StartMenu.startMenu();

    }

    void depleteResources()
    {
        power -= 0.01f;
        moderator -= power / 10000f;
        
    }


    public void SetIsPlayer()
    {
        isPlayer = true;
        powerBar.GetComponent<SpriteRenderer>().color = Color.cyan;
        powerBar.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), -gameObject.transform.eulerAngles.z);
        moderatorBar.transform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), -gameObject.transform.eulerAngles.z);
        powerBar.transform.localScale = new Vector3(1,0.25f,1);
        moderatorBar.transform.localScale = new Vector3(1, 0.25f, 1);
    }


    private void OnValidate()
    {
        if (changedValues)
        {
            attackDelay.Value = MaxAttackDelay;
            movementSpeed.Value = MaxMovementSpeed;
            turretRotateSpeed.Value = MaxTurretRotate;
            acceleration.Value = MaxAcceleration;
            treadsRotationSpeed.Value = MaxTreadsRotationSpeed;
            changedValues = false;
        }
        if (spawnPlayer)
        {
            SpawnPlayerBot();
            spawnPlayer = false;
        }
        
        
    }

    

    public GameObject SpawnBot()
    {
        Vector2 place = Random.insideUnitCircle;
        place = place.normalized;
        GameObject newBot = Instantiate(botPrefab, gameObject.transform.position + new Vector3(place.x, place.y, 0) * 2, Quaternion.identity);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        newBot.SetActive(true);
        newBot.GetComponent<RobotInputComponent>().ownerReactor = this;
        TerrainGenerator.entities.Add(newBot);
        robots.Add(newBot);
        newBot.GetComponent<TeamComponent>().SetPlayer(isPlayer);
        return newBot;
    }


    public GameObject SpawnPlayerBot()
    {
        GameObject newBot = SpawnBot();
        newBot.GetComponent<MoveAuthorityComponent>().SetAuthority(GetComponent<PlayerInputComponent>());
        newBot.name = "player";
        newBot.AddComponent<CameraLookComponent>();
        GetComponent<PlayerInputComponent>().ChangeControlledObject(newBot);
        TerrainGenerator.entities.Add(newBot);
        newBot.SetActive(true);
        robots.Add(newBot);

        return newBot;
    }

    void OnDestroy()
    {
        for (int i = robots.Count-1; i >= 0; i--)
        {
           Destroy(robots[i]);


        }
    }
}
