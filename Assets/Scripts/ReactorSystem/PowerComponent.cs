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
        if (gunTurretAimComponent)
        {
            gunTurretAimComponent.SetLookSpeed(turretAimSpeed);
        }
        if (shootingComponent)
        {
            shootingComponent.SetFireDelay(fireDelay);
        }
        if (moveComponent)
        {
            moveComponent.SetMovementSpeed(moveSpeed, acceleration);
        }
        if (treadsAimComponent)
        {
            treadsAimComponent.SetLookSpeed(treadsRotationSpeed);
        }
    }
}
