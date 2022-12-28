using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoEvent_mobMagicCaster : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip spitMagicLaser;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放發射雷射音效
    /// </summary>
    void PlaySpitMagicLaserEvent()
    {
        audioSource.PlayOneShot(spitMagicLaser, 1f);
    }
}
