using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip takeSword;
    public AudioClip swingSword;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(audioSource);
    }

    void PlaySwordEvent_take()
    {
        audioSource.PlayOneShot(takeSword, 0.5f);
    }

    void PlaySwordEvent_swing()
    {
        audioSource.PlayOneShot(swingSword, 0.5f);
    }
}
