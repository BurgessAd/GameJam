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
    public float power = 100;
    public float moderator = 100;
    

    public GameObject spinner;
    public GameObject spinningRods;



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


    void Awake()
    {
        attackDelay = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        movementSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        turretRotateSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        acceleration = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        treadsRotationSpeed = ScriptableObject.CreateInstance<SharedProperties>() as SharedProperties;
        changedValues = true;
        OnValidate();
        
        powerBar.transform.localScale = Vector3.zero;
        moderatorBar.transform.localScale = Vector3.zero;

        for (int i = 0; i < numBotsToSpawn; i++)
        {
            SpawnBot();
        }
    }


    void Update()
    {



        spinner.transform.Rotate(0, 0, power);
        spinningRods.transform.Rotate(0, 0, -power);
        if (isPlayer)
        {
            if (Input.GetKey(KeyCode.Space)&&(TerrainGenerator.player.transform.position-gameObject.transform.position).magnitude< 7)
            {

            }
            




            depleteResources();
            powerBar.transform.localScale = new Vector3(powerBar.transform.localScale.x * power / maxPower, powerBar.transform.localScale.y, powerBar.transform.localScale.z);
            moderatorBar.transform.localScale = new Vector3(moderatorBar.transform.localScale.x * moderator / maxModerator, moderatorBar.transform.localScale.y, moderatorBar.transform.localScale.z);
            if (robots.Count == 0)
            {
                GameOver();
            }
        }
    }

    public void killMe(GameObject _robot)
    {
        


        robots.Remove(_robot);
        if (_robot == TerrainGenerator.player)
        {
            int index = (int)Random.Range(0, robots.Count - 1);
            robots[index].GetComponent<MoveAuthorityComponent>().SetAuthority(GetComponent<PlayerInputComponent>());
            robots[index].GetComponent<CameraLookComponent>().ChangeCameraFocus(robots[index]);
            TerrainGenerator.player = robots[index]; 
        }
        Destroy(_robot,2.0f);
    }



    public void GameOver()
    {

    }

    void depleteResources()
    {
        power -= 0.0001f;
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
        
    }

    public void SpawnBot()
    {
        Vector2 place = Random.insideUnitCircle;
        place = place.normalized;
        GameObject newBot = Instantiate(botPrefab, gameObject.transform.position + new Vector3(place.x, place.y, 0) * 5, Quaternion.identity);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        newBot.SetActive(true);
        newBot.GetComponent<RobotInputComponent>().ownerReactor = this;
        robots.Add(newBot);
    }


    public GameObject SpawnPlayerBot()
    {
        Vector2 place = Random.insideUnitCircle;
        place = place.normalized;
        GameObject newBot = Instantiate(botPrefab, gameObject.transform.position + new Vector3(place.x, place.y, 0) * 5, Quaternion.identity);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        newBot.GetComponent<MoveAuthorityComponent>().SetAuthority(GetComponent<PlayerInputComponent>());
        newBot.GetComponent<RobotInputComponent>().ownerReactor = this;
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
