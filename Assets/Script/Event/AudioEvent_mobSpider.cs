using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobSpider : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip spiderBomb;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ����j���z������
    /// </summary>
    void PlaySpiderBombEvent()
    {
        audioSource.PlayOneShot(spiderBomb, 0.7f);
    }
}
