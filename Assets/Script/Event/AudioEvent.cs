using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip swingTorch;
    public AudioClip takeSword;
    public AudioClip swingSword;
    public AudioClip swingSword1;
    public AudioClip swingSword2;
    public AudioClip punchShield;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(audioSource);
    }

    /// <summary>
    /// 火把音效
    /// </summary>
    void PlayTorchEvent_swing()
    {
        audioSource.PlayOneShot(swingTorch, 1f);
    }

    /// <summary>
    /// 拿劍音效
    /// </summary>
    void PlaySwordEvent_take()
    {
        audioSource.PlayOneShot(takeSword, 0.5f);
    }

    /// <summary>
    /// 揮劍音效
    /// </summary>
    void PlaySwordEvent_swing()
    {
        audioSource.PlayOneShot(swingSword, 0.5f);
    }

    /// <summary>
    /// 揮細劍音效1
    /// </summary>
    void PlaySwordEvent_swing1()
    {
        audioSource.PlayOneShot(swingSword1, 0.5f);
    }
    /// <summary>
    /// 揮細劍音效2
    /// </summary>
    void PlaySwordEvent_swing2()
    {
        audioSource.PlayOneShot(swingSword2, 0.5f);
    }

    /// <summary>
    /// 盾擊音效
    /// </summary>
    void PlayPunchShield()
    {
        audioSource.PlayOneShot(punchShield, 0.6f);
    }
}
