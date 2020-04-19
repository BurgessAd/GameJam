using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    [SerializeField]
    float bulletDamage = 10.0f;
    [SerializeField]
    float bulletSpeed = 10.0f;
    [SerializeField]
    float bulletSurviveTime = 1.0f;
    float time;
    float currentBulletTime = 0.0f;
    private GameObject shooter;
    private Transform bulletTransform;
    void Awake()
    {
        time = Time.time;
        bulletTransform = GetComponent<Transform>();
    }

    void Update()
    {
        bulletTransform.localPosition += bulletTransform.up * bulletSpeed;
        currentBulletTime += 0.1f;
        if (currentBulletTime > bulletSurviveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Time.time - time>0.02f)
        {
            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
            if (collision.GetComponent<HealthComponent>())
            {
                collision.GetComponent<HealthComponent>().ProcessHit(bulletDamage);
            }
        }

        
    }
}
