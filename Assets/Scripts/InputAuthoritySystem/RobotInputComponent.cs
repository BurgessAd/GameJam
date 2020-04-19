using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInputComponent : InputComponent
{
    private float timer;
    private float wait;
    private Vector2 dir;
    public float visionDist = 5;
    public bool stayStill = false;


    //public ItemObject uranium;
    public ReactorComponent ownerReactor;


    public override Vector2 GetLookDirection()
    {
        return getRandomDir();
    }

    public override Vector2 GetMoveDirection()
    {
        if (stayStill)
        {
            return Vector2.zero;
        }
        else
        {
            return getRandomDir();
        }
        
    }

    void Start()
    {


        //gameObject.GetComponent<InventoryComponent>().AddItem(uranium, 2);
        
        gameObject.GetComponent<HealthComponent>().OnObjectDied += Die;

        
        
        timer = Time.time;
        wait = Random.Range(2, 5);
    }


    public void Die()
    {
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        ownerReactor.killMe(gameObject);
    }

    void FixedUpdate()
    {
        if ((TerrainGenerator.player.transform.position - gameObject.transform.position).magnitude < visionDist && ownerReactor!=TerrainGenerator.player.GetComponent<RobotInputComponent>().ownerReactor)
        {
            stayStill = true;
            dir = (Vector2)(TerrainGenerator.player.transform.position - gameObject.transform.position);

        }

        else if (Time.time - timer >wait)
        {
            stayStill = false;
            
            wait = Random.Range(0, 3);
            timer = Time.time;
            if(dir== Vector2.zero)
            {
                dir = Random.insideUnitCircle;
            }
            else
            {
                dir = Vector2.zero;
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
        return stayStill;
    }

}
