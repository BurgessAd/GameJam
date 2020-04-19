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

    bool changedValues;

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
        for (int i = 0; i < numBotsToSpawn; i++)
        {
            SpawnBot();
        }
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
        Debug.Log("!!!");
        GameObject newBot = Instantiate(botPrefab);
        newBot.GetComponent<PowerComponent>().SetComponentSharedProperties(turretRotateSpeed, attackDelay, movementSpeed, acceleration, treadsRotationSpeed);
        newBot.GetComponent<MoveAuthorityComponent>().SetAuthority(GetComponent<PlayerInputComponent>());
        newBot.SetActive(true);
    }
}
