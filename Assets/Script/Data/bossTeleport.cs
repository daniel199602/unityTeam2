﻿using System.Collections;
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

    GameManager gameManager = GameManager.Instance();

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
        PlayPortalEvent();//播音效
        ps.Play();

        Debug.Log("boss");
        GameManager.Instance().mobPool.Clear();
        FadeOutWait();
        //duration2 = 2f;
        //Invoke(nameof(playerclose), duration2);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F8))
        {
            WeaponManager.Instance.SetDefaultWeaponFirst(); //開始時設置預設武器(遊戲展示用)

            PlayPortalEvent();//播音效
            gameManager.mobPool.Clear();
            FadeOutWait();
            //作弊系統直接傳王關 不要亂按!!
        }
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
