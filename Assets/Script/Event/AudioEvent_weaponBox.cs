using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent_weaponBox : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip boxKnock;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �����_�c���Y����
    /// </summary>
    void PlayBoxKnockEvent()
    {
        audioSource.PlayOneShot(boxKnock, 0.7f);
    }
}
