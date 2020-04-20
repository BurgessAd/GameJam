using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInputComponent : InputComponent
{
    private float timer;
    private float wait;
    private Vector2 dir;
    public float visionDist = 10;
    public bool stayStill = false;
    public bool readjust = false;
    public Vector2 adjustDir;
    public GameObject target;
    public float readjustTimer=0;
    public ReactorComponent ownerReactor;
    int count = 0;




    public override Vector2 GetLookDirection()
    {

        return getRandomDir();

    }

    public override Vector2 GetMoveDirection()
    {
        if (readjust)
        {

            return adjustDir;
        }

        if (stayStill)
        {
            return Vector2.zero;
        }
        else
        {
            return getRandomDir();
        }
        
    }

    public void readjustPosition()
    {
        readjustTimer = Time.time;
        readjust = true;
        


    }


    void Start()
    {
        
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

        count++;
        if (count % 40 == 0)
        {
            if (target == null)
            {
                getTarget();
            }
            else if ((target.transform.position - gameObject.transform.position).magnitude > visionDist)
            {
                target = null;
            }
        }


        


        if (readjust && Time.time - readjustTimer > 2)
        {
            readjust = false;
        }


        if (target!=null &&(target.transform.position - gameObject.transform.position).magnitude < visionDist)
        {
            stayStill = true;
            dir = (Vector2)(target.transform.position - gameObject.transform.position);

        }

        else if (Time.time - timer >wait)
        {
            target = null;
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

    public void getTarget()
    {
        for (int i = 0; i < TerrainGenerator.entities.Count; i++)
        {
            GameObject go = TerrainGenerator.entities[i];
            if (!(go.GetComponent<EntityTag>().entityType == EntityTag.EntityType.Robot && go.GetComponent<RobotInputComponent>().ownerReactor==ownerReactor))
            {
                if ((go.transform.position - gameObject.transform.position).magnitude <= visionDist)
                {
                    target = go;
                    break;
                }
            }

        }
    }


    void OnDestroy()
    {
        TerrainGenerator.entities.Remove(gameObject);
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
