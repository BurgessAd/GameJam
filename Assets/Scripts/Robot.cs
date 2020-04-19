using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public static GameObject player;
    public int lookDistance;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 diff = (Vector2)(player.transform.position - gameObject.transform.position);
        if (diff.magnitude < lookDistance)
        {
            Shoot();
        }
    }

    void Shoot()
    {

    }
}
