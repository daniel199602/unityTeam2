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
        PlayPortalEvent();//������
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
            GameObject weapon = WeaponManager.Instance().ChooseAndUseWeaponTest(3, 31);//�]�mid31���Z��
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(weapon);//�]�m��e�Z���i�Z����

            PlayPortalEvent();//������
            GameManager.Instance().mobPool.Clear();
            FadeOutWait();
            //�@���t�Ϊ����Ǥ��� ���n�ë�!!
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
