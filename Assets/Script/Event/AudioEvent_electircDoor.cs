using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_electircDoor : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip electricity;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放雷電門音效
    /// </summary>
    public void PlayElectricityEvent()
    {
        audioSource.PlayOneShot(electricity,0.5f);
    }
}
