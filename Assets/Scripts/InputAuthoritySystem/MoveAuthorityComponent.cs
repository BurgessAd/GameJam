using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RobotInputComponent))]
public class MoveAuthorityComponent : MonoBehaviour
{
    [SerializeField]
    private AimComponent mouseLookDirection;
    [SerializeField]
    private MovementComponent moveDirection;
    [SerializeField]
    private ShooterComponent shootState;
    private InputComponent controllingInput;

    public void SetAuthority(InputComponent component)
    {
        controllingInput = component;
    }

    private void Update()
    {
        if (!controllingInput)
        {
            controllingInput = GetComponent<RobotInputComponent>();
        }
        mouseLookDirection.SetDesiredLookDirection(controllingInput.GetLookDirection());
        moveDirection.SetDesiredSpeed(controllingInput.GetMoveDirection());
        shootState.SetShootState(controllingInput.GetShootState());
    }
}
