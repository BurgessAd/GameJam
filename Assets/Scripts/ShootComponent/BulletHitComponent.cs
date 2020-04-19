using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitComponent : MonoBehaviour
{
    [SerializeField]
    SoundComponent soundComponent;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    Transform hittransform;

    private void Awake()
    {
        hittransform.Rotate(Vector3.forward, Random.Range(0, 360));
        soundComponent.Play(audioSource);
        StartCoroutine(hitRoutine());
    }
    IEnumerator hitRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
