using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInputComponent : InputComponent
{
    public override Vector2 GetLookDirection()
    {
        return Vector2.zero;
    }

    public override Vector2 GetMoveDirection()
    {
        return Vector2.zero;
    }

    public override bool GetPickUpState()
    {
        return false;
    }

    public override bool GetShootState()
    {
        return false;
    }

}
