using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip takeSword;
    public AudioClip swingSword;
    public AudioClip swingSword1;
    public AudioClip swingSword2;
    
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

    /// <summary>
    /// �ӼC����1
    /// </summary>
    void PlaySwordEvent_swing1()
    {
        audioSource.PlayOneShot(swingSword1, 0.5f);
    }
    /// <summary>
    /// �ӼC����2
    /// </summary>
    void PlaySwordEvent_swing2()
    {
        audioSource.PlayOneShot(swingSword2, 0.5f);
    }
}
