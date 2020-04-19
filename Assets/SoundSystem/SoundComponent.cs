using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    [Range(0.0f, 3.0f)]
    public float pitchDefault= 0.5f;
    [Range(0.0f, 1.0f)]
    public float volDefault = 0.5f;

    [Range(0.0f, 1.0f)]
    public float pitchDiff;
    [Range(0.0f, 1.0f)]
    public float volDiff;
    public AudioClip[] clips;

    public void Play(AudioSource source)
    {
        if (clips.Length == 0)
        {
            return;
        }
        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = Mathf.Clamp(volDefault + 0.5f * Mathf.Sign(Random.Range(-1,1))*Random.Range(0, volDiff), 0, 1);
        source.pitch = Mathf.Clamp(pitchDefault + 0.5f * Mathf.Sign(Random.Range(-1, 1)) * Random.Range(0, pitchDiff), 0, 1);
        source.Play();
    }
}
