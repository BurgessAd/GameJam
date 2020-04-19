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
        if ((TerrainGenerator.player.transform.position - gameObject.transform.position).magnitude < visionDist)
        {
            
            dir = (Vector2)(TerrainGenerator.player.transform.position - gameObject.transform.position);

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
