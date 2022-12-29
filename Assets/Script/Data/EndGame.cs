using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    bool isInZone;
    bool EndVFXing;
    public GameObject EndVFX;
    public AudioClip portal;
    public GameObject fadeColor_ForEnd;
    GameObject fog;


    ParticleSystem ps;
    AudioSource audioSource;
    Animation fadeEnd;
    // Start is called before the first frame update
    void Start()
    {
        fog = GameObject.Find("FogOfWarPlane");
        isInZone = false;
        EndVFXing = false;
        ps = EndVFX.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        fadeEnd = fadeColor_ForEnd.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&& isInZone)
        {
            if (EndVFXing == false)
            {
                PlayPortalEvent();
                ps.Play();
                fadeEnd.Play("FadeEnd");
                EndVFXing = true;
            }
            Invoke(nameof(EndToGameMenu), 3);
            

        }
    }

    void EndToGameMenu()
    {
        fog.GetComponent<MeshRenderer>().materials[0].SetFloat("_FogRadius", 80);
        SceneManager.LoadScene("GameMenu");
        UIManager.Instance().GameMenuPanelOpen();
        UIManager.Instance().QuitGameUIClose();
        GameManager.Instance().MobPoolClear();
        GameManager.Instance().PlayerSetActiveSwitch(false);
        GameManager.Instance().audioSource.clip = GameManager.Instance().audios[0];
        GameManager.Instance().audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        isInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInZone = false;
    }
    void PlayPortalEvent()
    {
        audioSource.PlayOneShot(portal);
    }
}
