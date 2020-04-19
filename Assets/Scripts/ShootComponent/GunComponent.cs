using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour
{
    private AttackAnimator attackAnimator;

    [SerializeField]
    private GameObject bulletPrefab;


    private Transform gunTransform;

    void Awake()
    {
        gunTransform = GetComponent<Transform>();
        attackAnimator = GetComponent<AttackAnimator>();
    }

    public void FireWeapon()
    {
        Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation).SetActive(true);
    }
}
