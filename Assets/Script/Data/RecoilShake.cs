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
    public void camraPlayerSake()
    {
        screenShake.GenerateImpulse(new Vector3(Random.Range(1,2), Random.Range(0,1), Random.Range(1,2)));
        Debug.Log("shake");
    }
    public void camraBearSake()
    {
        screenShake.GenerateImpulse(new Vector3(Random.Range(1,2), Random.Range(0,1), Random.Range(1,2)));
        Debug.Log("shake");
    }
    void Update()
    {

    }
}
