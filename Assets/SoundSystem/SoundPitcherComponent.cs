using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPitcherComponent : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;
    bool isActive = false;
    [SerializeField]
    float maxPitch;
    [SerializeField]
    float minPitch;
    [SerializeField]
    AudioClip clipToPitch;

    public void SetActive(bool activeState)
    {
        if (isActive != activeState)
        {
            isActive = activeState;
            if (activeState)
            {
                source.clip = clipToPitch;
                source.Play();
            }
            else
            {
                source.Stop();
            }
        }
    }

    public void SetPitch(float newPitchMult)
    {
        source.pitch = minPitch + (maxPitch - minPitch) * newPitchMult;
    }
}
