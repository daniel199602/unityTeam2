using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    private void Start()
    {
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("shake");
            screenShake.GenerateImpulse();
        }
    }
}
