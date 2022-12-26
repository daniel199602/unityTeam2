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
            GameObject weapon = WeaponManager.Instance().ChooseAndUseWeaponTest(3, 31);//設置id31號武器
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(weapon);//設置當前武器進武器格

            PlayPortalEvent();//播音效
            GameManager.Instance().mobPool.Clear();
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
