using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bossTeleport : MonoBehaviour
{
    Animation fadeOut;

    void Awake()
    {
        fadeOut = GameObject.Find("FadeColor").GetComponent<Animation>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        fadeOut.Play("FadeOut");
        Debug.Log("boss");
    }
}
