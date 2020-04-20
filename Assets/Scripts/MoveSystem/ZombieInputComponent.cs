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
    public float visionDist = 5;
    public Animator animator;
    private float attackTimer;
    private float attackDelay = 0.5f;



    public override Vector2 GetLookDirection()
    {
        return getRandomDir();
    }

    public override Vector2 GetMoveDirection()
    {
            return getRandomDir();
    }

    void Start()
    {
        attackTimer = Time.time;
        SharedProperties speed = new SharedProperties();
        speed.Value = 10f;
        SharedProperties acc = new SharedProperties();
        acc.Value = 1f;
        movementComponent.SetMovementSpeed(speed,acc);
        SharedProperties rot = new SharedProperties();
        rot.Value = 6f;
        lookdir.SetLookSpeed(rot);

        //gameObject.GetComponent<InventoryComponent>().AddItem(uranium, 2);

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


        animator.SetFloat("Speed", dir.magnitude);
        if ((TerrainGenerator.player.transform.position - gameObject.transform.position).magnitude < visionDist)
        {
            
            dir = (Vector2)(TerrainGenerator.player.transform.position - gameObject.transform.position);
            if((TerrainGenerator.player.transform.position - gameObject.transform.position).magnitude < 3)
            {
                animator.SetBool("Attacking", true);
;            }
            else
            {
                animator.SetBool("Attacking", false);
            }

        }

        else if (Time.time - timer > wait)
        {
            

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

        movementComponent.SetDesiredSpeed(dir);
        lookdir.SetDesiredLookDirection(-dir);

    }

    public void OnCollisionStay2D(Collision2D c)
    {
        
        if (c.gameObject.name == "player" &&Time.time-attackTimer>attackDelay)
        {
            Debug.Log("HIT");
            attackTimer = Time.time;
            c.gameObject.GetComponent<HealthComponent>().ProcessHit(5f);
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
