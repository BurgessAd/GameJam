using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    Vector3 desiredMoveSpeed;
    Vector3 currentMoveSpeed;

    private Rigidbody2D body;

    [SerializeField]
    [Range(0, 10.0f)]
    float movementSpeed;

    [SerializeField]
    [Range(0, 0.1f)]
    float moveChangePerFrame;


    AnimationCurve accelerationCurve;


    AnimationCurve deccelerationCurve;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Animate();
    }

    public void SetDesiredSpeed(Vector2 newDesiredSpeed)
    {
        desiredMoveSpeed = newDesiredSpeed;

        if (desiredMoveSpeed.magnitude > 1.0f)
        {
            desiredMoveSpeed.Normalize();
        }
    }

    private void Move()
    {
        currentMoveSpeed.x = Mathf.Clamp(currentMoveSpeed.x + Mathf.Sign(desiredMoveSpeed.x - currentMoveSpeed.x) * Mathf.Clamp(Mathf.Abs(desiredMoveSpeed.x - currentMoveSpeed.x), 0, moveChangePerFrame), -1.0f, 1.0f);
        currentMoveSpeed.y = Mathf.Clamp(currentMoveSpeed.y + Mathf.Sign(desiredMoveSpeed.y - currentMoveSpeed.y) * Mathf.Clamp(Mathf.Abs(desiredMoveSpeed.y - currentMoveSpeed.y), 0, moveChangePerFrame), -1.0f, 1.0f);

        body.velocity = new Vector2(currentMoveSpeed.x * movementSpeed, currentMoveSpeed.y * movementSpeed);
    }

    private void Animate()
    {

    }
}
