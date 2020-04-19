using System.Collections;
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


    private List<GameObject> robots = new List<GameObject>();

    [SerializeField]
    int numBotsToSpawn = 1;

    [SerializeField]
    GameObject botPrefab;

    [SerializeField]
    [Range(0.1f,2f)]
    private float MaxAttackDelay = 1.0f;
    [SerializeField]
    [Range(1f,4f)]
    private float MaxMovementSpeed = 3.0f;
    [SerializeField]
    [Range(1f,4f)]
    private float MaxTurretRotate = 3.0f;
    [SerializeField]
    [Range(0.01f,0.5f)]
    private float MaxAcceleration = 0.5f;
    [SerializeField]
    [Range(1f, 4f)]
    private float MaxTreadsRotationSpeed = 3.0f;
    [SerializeField]
    bool changedValues;


    public bool isPlayer = false;

    [SerializeField]
    bool spawnPlayer;


    void Awake()
    {
        attackDelay = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        movementSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        turretRotateSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        acceleration = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        treadsRotationSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        changedValues = true;
        OnValidate();
        Debug.Log("!");
        powerBar.transform.localScale = Vector3.zero;
        moderatorBar.transform.localScale = Vector3.zero;

        for (int i = 0; i < numBotsToSpawn; i++)
        {
            SpawnBot();
        }
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

    public void SpawnBot()
    {
        Vector2 place = Random.insideUnitCircle;
        place = place.normalized;
        GameObject newBot = Instantiate(botPrefab, gameObject.transform.position + new Vector3(place.x, place.y, 0) * 5, Quaternion.identity);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        newBot.SetActive(true);
        robots.Add(newBot);
    }


    public GameObject SpawnPlayerBot()
    {
        Vector2 place = Random.insideUnitCircle;
        place = place.normalized;
        GameObject newBot = Instantiate(botPrefab, gameObject.transform.position + new Vector3(place.x, place.y, 0) * 5, Quaternion.identity);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        GetComponent<PlayerInputComponent>().ChangeControlledObject(newBot);
        newBot.SetActive(true);
        robots.Add(newBot);
        return newBot;
    }

    void OnDestroy()
    {
        for (int i = 0; i < robots.Count; i++)
        {
            Destroy(robots[i]);
        }
    }
}
