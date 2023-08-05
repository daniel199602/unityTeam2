using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_frogTrap : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip frogSpitFire;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放青蛙陷阱噴火音效
    /// </summary>
    public void PlayFrogSpitFireEvent()
    {
        audioSource.PlayOneShot(frogSpitFire, 0.5f);
    }

    //public void PlayForTime(float time)
    //{
    //    audioSource.Play();
    //    Invoke("StopAudio", time);
    //}

    //private void StopAudio()
    //{
    //    audioSource.Stop();

    //}
}
