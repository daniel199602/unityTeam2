using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobBear : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip bearGrowl;
    public AudioClip bearBite;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ���񺵧q����
    /// </summary>
    public void PlayBearGrowlEvent()
    {
        audioSource.PlayOneShot(bearGrowl, 0.5f);
    }

    /// <summary>
    /// ���񺵫r������
    /// </summary>
    public void PlayBearBiteEvent()
    {
        audioSource.PlayOneShot(bearBite, 0.5f);
    }
}
