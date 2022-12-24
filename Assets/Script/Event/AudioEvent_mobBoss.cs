using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_mobBoss : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip swingAx;
    public AudioClip swingMetalSword_1;
    public AudioClip swingMetalSword_2;
    public AudioClip fireRoad;
    public AudioClip fireBall_1;
    public AudioClip fireBall_2;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 斧擊音效
    /// </summary>
    void PlaySwingAxEvent_swing()
    {
        audioSource.PlayOneShot(swingAx, 1f);
    }

    /// <summary>
    /// 劍擊音效_1
    /// </summary>
    void PlaySwingMetalSwordEvent_1()
    {
        audioSource.PlayOneShot(swingMetalSword_1, 1f);
    }

    /// <summary>
    /// 劍擊音效_2
    /// </summary>
    void PlaySwingMetalSwordEvent_2()
    {
        audioSource.PlayOneShot(swingMetalSword_2, 1f);
    }

    /// <summary>
    /// 火炎道路音效
    /// </summary>
    void PlayFireRoadEvent()
    {
        audioSource.PlayOneShot(fireRoad, 1f);
    }

    /// <summary>
    /// 火焰球用於切換到狀態二音效_1
    /// </summary>
    void PlayFireBallStageTowEvent_1()
    {
        audioSource.PlayOneShot(fireBall_1, 1f);
    }

    /// <summary>
    /// 火焰球用於噴火音效_2
    /// </summary>
    void PlayFireBallEvent_2()
    {
        audioSource.PlayOneShot(fireBall_2, 1f);
    }
}
