using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAuthorityComponent : MonoBehaviour
{
    [SerializeField]
    private AimComponent mouseLookDirection;
    [SerializeField]
    private MovementComponent moveDirection;
    [SerializeField]
    private ShooterComponent shootState;
    [SerializeField]
    private PlayerInputComponent controllingInput;

    public void SetAuthority(PlayerInputComponent component)
    {
        controllingInput = component;
        controllingInput = GetComponent<PlayerInputComponent>();
    }

    private void Update()
    {
        if (!controllingInput)
        {
            controllingInput = GetComponent<PlayerInputComponent>();
        }
        mouseLookDirection.SetDesiredLookDirection(controllingInput.GetLookDirection());
        moveDirection.SetDesiredSpeed(controllingInput.GetMoveDirection());
        shootState.SetShootState(controllingInput.GetShootState());
    }
}
