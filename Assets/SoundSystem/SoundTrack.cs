using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrack : MonoBehaviour
{

    public AudioSource ads;
    // Start is called before the first frame update
    void Start()
    {
        ads.Play();
        DontDestroyOnLoad(gameObject);
    }

    
}
