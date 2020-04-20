using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    Vector3 desiredMoveSpeed;
    Vector3 currentMoveSpeed;

    private Rigidbody2D body;

    private SharedProperties movementSpeed;
    private SharedProperties acceleration;

    [SerializeField]
    AnimationCurve accelerationCurve;

    [SerializeField]
    AnimationCurve deccelerationCurve;

    private LowerAnimator lowerAnimator;

    private SoundPitcherComponent soundPitcher;

    public void SetMovementSpeed(SharedProperties movementSpeed, SharedProperties acceleration)
    {
        this.movementSpeed = movementSpeed;
        this.acceleration = acceleration;
    }

    private void Awake()
    {
        if (GetComponentInChildren<LowerAnimator>() != null) {
            lowerAnimator = GetComponentInChildren<LowerAnimator>();
        }
        else
        {
            lowerAnimator = gameObject.GetComponent<LowerAnimator>();
        }
        SharedProperties s = ScriptableObject.CreateInstance("SharedProperties") as SharedProperties;
        s.Value = 1;
        SetMovementSpeed(s,s);


        body = GetComponent<Rigidbody2D>();
        
        soundPitcher = GetComponent<SoundPitcherComponent>();
        if (soundPitcher != null)
        {
            soundPitcher.SetActive(true);
        }
        
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        lowerAnimator.SetAnimationState(body.velocity);
        if (soundPitcher!=null)
        {
            soundPitcher.SetPitch(body.velocity.magnitude);
        }
        
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
        if (desiredMoveSpeed != null)
        {
            currentMoveSpeed.x = Mathf.Clamp(currentMoveSpeed.x + Mathf.Sign(desiredMoveSpeed.x - currentMoveSpeed.x) * Mathf.Clamp(Mathf.Abs(desiredMoveSpeed.x - currentMoveSpeed.x), 0, acceleration.Value), -1.0f, 1.0f);
            currentMoveSpeed.y = Mathf.Clamp(currentMoveSpeed.y + Mathf.Sign(desiredMoveSpeed.y - currentMoveSpeed.y) * Mathf.Clamp(Mathf.Abs(desiredMoveSpeed.y - currentMoveSpeed.y), 0, acceleration.Value), -1.0f, 1.0f);
            body.velocity = new Vector2(currentMoveSpeed.x * movementSpeed.Value, currentMoveSpeed.y * movementSpeed.Value);
        }

    }
}
