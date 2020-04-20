using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInputComponent : InputComponent
{
    public override Vector2 GetLookDirection()
    {
        return new Vector2(0, -1);
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
        return true;
    }

}
