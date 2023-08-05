using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobSpider : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip spiderBomb;
    public AudioClip spiderBrust;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 播放蜘蛛爆炸音效
    /// </summary>
    void PlaySpiderBombEvent()
    {
        audioSource.PlayOneShot(spiderBomb, 0.7f);
    }

    /// <summary>
    /// 播放蜘蛛叫聲爆炸音效
    /// </summary>
    void PlaySpiderBrustEvent()
    {
        audioSource.PlayOneShot(spiderBrust, 0.7f);
    }
}
