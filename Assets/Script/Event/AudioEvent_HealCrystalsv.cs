using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_HealCrystalsv : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip healHp;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 治癒音效
    /// </summary>
    void PlayHealHpEvent()
    {
        audioSource.PlayOneShot(healHp, 1f);
    }
}
