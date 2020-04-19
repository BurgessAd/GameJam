using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public static GameObject player;
    public int lookDistance= 5;
    public bool isPlayer;
    public float timer;
    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time ;
        waitTime = 0;
        isPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPlayer)
        {
            if (Time.time-timer> waitTime)
            {
                
                setNewMoveDirection();
            }
            

        }



        Vector2 diff = (Vector2)(player.transform.position - gameObject.transform.position);
        if (diff.magnitude < lookDistance)
        {
            Shoot();
        }
    }

    void LateUpdate()
    {
        if(gameObject == player)
        {

        }
    }


    void setNewMoveDirection()
    {

    }

    void Shoot()
    {

    }
}
