using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour
{
    private Animator attackAnimator;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float animationTimeMultiplier = 1.0f;
    private Transform gunTransform;

    void Awake()
    {
        gunTransform = GetComponent<Transform>();
        attackAnimator = GetComponent<Animator>();
    }

    public void FireWeapon(float fireDelay)
    {
        attackAnimator.SetFloat("FireSpeed", 1 / fireDelay * animationTimeMultiplier);
        attackAnimator.Play("FireAnimation", -1, 0);
        Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation).SetActive(true);
    }
}
