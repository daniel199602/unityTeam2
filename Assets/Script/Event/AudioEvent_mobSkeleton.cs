using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobSkeleton : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip windBorn;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放出生的風音效
    /// </summary>
    public void PlayWindBornEvent()
    {
        audioSource.PlayOneShot(windBorn, 0.5f);
    }
}
