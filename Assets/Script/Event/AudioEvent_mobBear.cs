using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobBear : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip bearGrowl;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ¼½©ñºµ§q­µ®Ä
    /// </summary>
    public void PlayBearGrowlEvent()
    {
        audioSource.PlayOneShot(bearGrowl, 0.5f);
    }
}
