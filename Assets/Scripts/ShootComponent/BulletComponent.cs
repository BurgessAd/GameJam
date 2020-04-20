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

    [SerializeField]
    private Transform bulletTransform;
    [SerializeField]
    private GameObject hitAnimator;
    [SerializeField]
    private Rigidbody2D bulletBody;

    void Update()
    {
        bulletBody.velocity = bulletTransform.up * bulletSpeed;
        currentBulletTime += 0.1f;
        if (currentBulletTime > bulletSurviveTime)
        {
            Destroy(gameObject);
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<Collider>().GetComponent<HealthComponent>())
    //    {
    //        collision.GetComponent<Collider>().GetComponent<HealthComponent>().ProcessHit(bulletDamage);
    //    }
    //    Instantiate(hitAnimator, bulletTransform.position, bulletTransform.rotation);
    //    Destroy(gameObject);
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<HealthComponent>())
        {
            collision.collider.GetComponent<HealthComponent>().ProcessHit(bulletDamage);
        }
        Instantiate(hitAnimator, bulletTransform.position, bulletTransform.rotation);
        Destroy(gameObject);
    }
}
