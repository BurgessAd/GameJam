using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputComponent : InputComponent
{
    private Transform targetTransformComponent;
    void Awake()
    {
        targetTransformComponent = GetComponent<Transform>();  
    }

    public void ChangeControlledObject(GameObject newObject)
    {
        newObject.GetComponent<MoveAuthorityComponent>().SetAuthority(this);
        targetTransformComponent = newObject.GetComponent<Transform>();
    }

    public override Vector2 GetLookDirection()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetTransformComponent.position;
    }





    public override Vector2 GetMoveDirection()
    {
        Vector2 desiredMovementSpeed = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            desiredMovementSpeed.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            desiredMovementSpeed.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            desiredMovementSpeed.x += 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            desiredMovementSpeed.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.H))
        {
            desiredMovementSpeed = gameObject.transform.position - targetTransformComponent.position;
        }

        return desiredMovementSpeed;
    }

    public override bool GetShootState()
    {
        return Input.GetMouseButton(0);
    }

    public override bool GetPickUpState()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
