using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputComponent : MonoBehaviour
{
    public abstract Vector2 GetLookDirection();
    public abstract Vector2 GetMoveDirection();
    public abstract bool GetShootState();
    public abstract bool GetPickUpState();
}
