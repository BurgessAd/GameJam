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

    float currentBulletTime = 0.0f;

    private Transform bulletTransform;
    void Awake()
    {
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
        Destroy(gameObject);
        if (collision.GetComponent<HealthComponent>())
        {
            collision.GetComponent<HealthComponent>().ProcessHit(bulletDamage);
        }
    }
}
