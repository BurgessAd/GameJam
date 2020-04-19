using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleExplosion : MonoBehaviour
{
    Animator explosionAnimation;
    SoundComponent soundComponent;
    AudioSource audioSource;
    [SerializeField]
    [Range(0f, 1f)]
    public float explosionSize = 1.0f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        explosionAnimation = GetComponent<Animator>();
        soundComponent = GetComponent<SoundComponent>();
        SetSize(explosionSize);
        Begin();
    }
    IEnumerator ExplosionCoroutine()
    {
        soundComponent.Play(audioSource);
        explosionAnimation.SetTrigger("explode");
        yield return new WaitForSeconds(1.4f);
        Destroy(gameObject);
    }
    public void Begin()
    {
        transform.localScale = transform.localScale * Random.Range(0.8f, 1.2f);
        StartCoroutine(ExplosionCoroutine());
    }

    public void SetSize(float size)
    {
        transform.localScale = transform.localScale * size;
        soundComponent.volDefault = size;
        soundComponent.pitchDefault = 0.5f + 3*(1-size);
    }
}
