using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bossTeleport : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip portal;

    Animation fadeOut;
    GameObject Player;
    public GameObject TeleportVFX;

    private float duration;
    private float duration2;
    void Awake()
    {
        fadeOut = GameObject.Find("FadeColor").GetComponent<Animation>();
        Player = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem ps = TeleportVFX.GetComponent<ParticleSystem>();
        PlayPortalEvent();//¼½­µ®Ä
        ps.Play();

        Debug.Log("boss");
        GameManager.Instance().mobPool.Clear();
        FadeOutWait();
        duration2 = 2f;
        Invoke(nameof(playerclose), duration2);
    }


    void FadeOutWait()
    {
        fadeOut.Play("FadeOut");
    }
    void playerclose()
    {
        Player.SetActive(false);
    }
    void PlayPortalEvent()
    {
        audioSource.PlayOneShot(portal);
    }
}
