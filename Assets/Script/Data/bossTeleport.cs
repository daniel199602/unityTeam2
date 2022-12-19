using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bossTeleport : MonoBehaviour
{
    Animation fadeOut;
    GameObject Player;
    public GameObject TeleportVFX;
    


    private float duration;
    private float duration2;
    void Awake()
    {
        fadeOut = GameObject.Find("FadeColor").GetComponent<Animation>();
        Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem ps = TeleportVFX.GetComponent<ParticleSystem>();
        
        ps.Play();

        Debug.Log("boss");
        GameManager.Instance().mobPool.Clear();
        duration = 0.5f;
        Invoke(nameof(FadeOutWait), duration);
        duration2 = 2.5f;
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
}
