using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bossTeleport : MonoBehaviour
{
    Animation fadeOut;
    GameObject Player;

    private float duration;
    void Awake()
    {
        fadeOut = GameObject.Find("FadeColor").GetComponent<Animation>();
        Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        
        fadeOut.Play("FadeOut");
        Debug.Log("boss");
        GameManager.Instance().mobPool.Clear();
        duration = 2;
        Invoke(nameof(playerclose), duration);
    }
    void playerclose()
    {
        Player.SetActive(false);
    }
}
