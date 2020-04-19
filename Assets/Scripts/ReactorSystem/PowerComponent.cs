using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerComponent : MonoBehaviour
{
    [SerializeField]
    private AimComponent gunTurretAimComponent;
    [SerializeField]
    private ShooterComponent shootingComponent;
    [SerializeField]
    private MovementComponent moveComponent;
    [SerializeField]
    private AimComponent treadsAimComponent;
    
    public void SetComponentSharedProperties(SharedProperties turretAimSpeed, SharedProperties fireDelay, SharedProperties moveSpeed, SharedProperties acceleration, SharedProperties treadsRotationSpeed)
    {
        gunTurretAimComponent.SetLookSpeed(turretAimSpeed);
        shootingComponent.SetFireDelay(fireDelay);
        moveComponent.SetMovementSpeed(moveSpeed, acceleration);
        treadsAimComponent.SetLookSpeed(treadsRotationSpeed);
    }
}
