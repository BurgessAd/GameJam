using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    private Transform aimTransform;

    private Vector2 desiredLookDirection;
    private Vector2 currentLookDirection;
    private SharedProperties lookSpeed;

    public void SetLookSpeed(SharedProperties lookSpeed)
    {
        this.lookSpeed = lookSpeed;
    }

    void Awake()
    {
        aimTransform = GetComponent<Transform>();
        if (lookSpeed == null)
        {
            lookSpeed = ScriptableObject.CreateInstance(typeof(SharedProperties)) as SharedProperties;
            lookSpeed.Value = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        currentLookDirection = transform.up;
        float angleBetween = Vector3.Angle(currentLookDirection, desiredLookDirection);
        int sign = -(int)Mathf.Sign(Vector3.Dot(transform.right, desiredLookDirection));
        aimTransform.Rotate(Vector3.forward, sign * Mathf.Min(angleBetween, lookSpeed.Value));
        //aimTransform.eulerAngles = Vector3.RotateTowards(currentLookDirection, desiredLookDirection, lookSpeed.Value, 0);  
    }

    public void SetDesiredLookDirection(Vector2 newLookDirection)
    {
        desiredLookDirection = newLookDirection;
    }
}
