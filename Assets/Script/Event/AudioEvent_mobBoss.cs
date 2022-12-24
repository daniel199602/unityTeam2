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



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ��������
    /// </summary>
    void PlaySwingAxEvent_swing()
    {
        audioSource.PlayOneShot(swingAx, 1f);
    }

    /// <summary>
    /// �C������_1
    /// </summary>
    void PlaySwingMetalSwordEvent_1()
    {
        audioSource.PlayOneShot(swingMetalSword_1, 1f);
    }

    /// <summary>
    /// �C������_2
    /// </summary>
    void PlaySwingMetalSwordEvent_2()
    {
        audioSource.PlayOneShot(swingMetalSword_2, 1f);
    }

    /// <summary>
    /// �Q������
    /// </summary>
    void PlayFireRoadEvent()
    {
        audioSource.PlayOneShot(fireRoad, 1f);
    }


}
