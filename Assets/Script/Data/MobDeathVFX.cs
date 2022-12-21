using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDeathVFX : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    public GameObject bearMaterial;
    public GameObject FireFVX;
    public GameObject FireFVX2;
    public AnimationCurve fadeIn;
    public AnimationCurve fadeOut;

    public float spawnEffectTime = 1;
    public float t=0.0f;
    public bool isDeath;
    bool deathIngVFX;
    private void Awake()
    {
        meshRenderer = bearMaterial.GetComponent<SkinnedMeshRenderer>();
        deathIngVFX = false;
    }
    void DeathFVXEvent()
    {
        isDeath = true;
    }
    void DeathFVX()
    {
        
        if (deathIngVFX==false)
        {
            Debug.Log("death");
            Material[] mats = meshRenderer.materials;
            t+= Time.deltaTime;
            //mats[0].SetFloat("_Cutoff", Mathf.Lerp(0, 1, t));
            mats[0].SetFloat("_Cutoff", fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, t)));
            meshRenderer.materials = mats;
        }
    }
    void DeathStart()
    {
        t = 0;
        deathIngVFX = false;
        Debug.Log("-----------------------------------------------------------deathStart");
    }
    void DeathEnd()
    {
        deathIngVFX = true;
        Debug.Log("-----------------------------------------------------------deathEnd");
    }
    void BornFVX()
    {
        t = 0.01f;
        Debug.Log("born");
        Material[] mats = meshRenderer.materials;
        t += Time.deltaTime;
        //mats[0].SetFloat("_Cutoff", Mathf.Lerp(0, 1, t));
        mats[0].SetFloat("_Cutoff", fadeOut.Evaluate(Mathf.InverseLerp(spawnEffectTime, 0, t)));
        meshRenderer.materials = mats;
        
        Debug.Log("-----------------------------------------------------------------------------------------------Born");
    }

    void DeathEvent_PS()
    {
        ParticleSystem ps = FireFVX.GetComponent<ParticleSystem>();
        ps.Play();
    }
    void DeathEvent_PS2()
    {
        ParticleSystem ps = FireFVX2.GetComponent<ParticleSystem>();
        ps.Play();
    }
    private void Update()
    {
        if(isDeath==true)
        {
            DeathFVX();
        }
    }


}
