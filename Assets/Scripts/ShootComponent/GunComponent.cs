using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunComponent : MonoBehaviour
{
    private Animator attackAnimator;

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float animationTimeMultiplier = 1.0f;
    private Transform gunTransform;
    private AudioSource audioSource;
    private SoundComponent soundComponent;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gunTransform = GetComponent<Transform>();
        attackAnimator = GetComponent<Animator>();
        soundComponent = GetComponent<SoundComponent>();
    }

    public void FireWeapon(float fireDelay)
    {
        attackAnimator.SetFloat("FireSpeed", 1 / fireDelay * animationTimeMultiplier);
        attackAnimator.Play("FireAnimation", -1, 0);
        soundComponent.Play(audioSource);
        Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation).SetActive(true);
    }
}
