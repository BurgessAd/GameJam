using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputComponent : MonoBehaviour
{
    private Transform targetTransformComponent;
    void Awake()
    {
        targetTransformComponent = GetComponent<Transform>();  
    }

    private void Start()
    {
        ChangeControlledObject(gameObject);
    }

    public void ChangeControlledObject(GameObject newObject)
    {
        newObject.GetComponent<MoveAuthorityComponent>().SetAuthority(this);
    }

    public Vector2 GetLookDirection()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition) - targetTransformComponent.position;
    }

    public Vector2 GetMoveDirection()
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
        return desiredMovementSpeed;
    }

    public bool GetShootState()
    {
        return Input.GetMouseButton(0);
    }
}
