using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieInputComponent : InputComponent
{

    public MovementComponent movementComponent;
    public AimComponent lookdir;
    // Start is called before the first frame update
    private float timer;
    private float wait;
    private Vector2 dir;
    public float visionDist = 10;
    public Animator animator;
    private float attackTimer;
    private float attackDelay = 0.5f;
    private bool attacking = false;
    public GameObject target;
    public float readjustTimer = 0;
    public bool readjusting = false;
    public override Vector2 GetLookDirection()
    {
        return getRandomDir();
    }

    public override Vector2 GetMoveDirection()
    {
            return getRandomDir();
    }



    public void readjust()
    {
        readjusting = true;
    }

    void Start()
    {

        GetComponent<CircleCollider2D>().radius = visionDist;
        attackTimer = Time.time;
        SharedProperties speed = new SharedProperties();
        speed.Value = 10f;
        SharedProperties acc = new SharedProperties();
        acc.Value = 1f;
        movementComponent.SetMovementSpeed(speed,acc);
        SharedProperties rot = new SharedProperties();
        rot.Value = 6f;
        lookdir.SetLookSpeed(rot);

        

        gameObject.GetComponent<HealthComponent>().OnObjectDied += Die;



        timer = Time.time;
        wait = Random.Range(2, 5);
    }


    public void Die()
    {
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        
    }

    void FixedUpdate()
    {

        if (readjusting && Time.time - readjustTimer > 1)
        {
            readjustTimer = Time.time;
            dir = transform.right;
            readjusting = false;

        }


        animator.SetFloat("Speed", dir.magnitude);
        if (target!=null&&(target.transform.position - gameObject.transform.position).magnitude < visionDist)
        {
            
            if(Time.time - readjustTimer > 3)
            {
                dir = (Vector2)(target.transform.position - gameObject.transform.position);
            }
            
            
            animator.SetBool("Attacking", attacking);
                
;            

        }

        else if (Time.time - timer > wait)
        {
            target = null;

            wait = Random.Range(0, 3);
            timer = Time.time;
            if (dir == Vector2.zero)
            {
                dir = Random.insideUnitCircle;
            }
            else
            {
                dir = Vector2.zero;
            }
        }
        if (attacking)
        {
            if ( Time.time - attackTimer > attackDelay)
            {
                attackTimer = Time.time;
                HealthComponent health = target.GetComponent<HealthComponent>();
                if (health)
                {
                    health.ProcessHit(5.0f);
                }
            }
            if (readjusting)
            {
                movementComponent.SetDesiredSpeed(dir);
            }
            else
            {
                movementComponent.SetDesiredSpeed(Vector2.zero);
            }
            
        }
        else
        {
            movementComponent.SetDesiredSpeed(dir);
        }
        lookdir.SetDesiredLookDirection(-dir);

    }
    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.name == "Tilemap")
        {
            readjust();
        }

        if (c.gameObject == target)
        {
            
            attacking = true;
        }
    }

    public void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject == target)
        {
            
            attacking = false;
        }
    }

    

    public void OnTriggerStay2D(Collider2D c)
    {
        if (target == null && c.gameObject.GetComponent<EntityTag>()!=null)
        {
            if (c.gameObject.GetComponent<EntityTag>().entityType == EntityTag.EntityType.Robot)
            {
                target = c.gameObject;
            }
            
        }
    }



    private Vector2 getRandomDir()
    {
        return dir;
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
