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
   
    private void Awake()
    {
        meshRenderer = bearMaterial.GetComponent<SkinnedMeshRenderer>();
        
    }
    void DeathFVXEvent()
    {
        isDeath = true;
    }
    void DeathFVX()
    {
        Debug.Log("death");
        Material[] mats = meshRenderer.materials;
        t += Time.deltaTime;
        //mats[0].SetFloat("_Cutoff", Mathf.Lerp(0, 1, t));
        mats[0].SetFloat("_Cutoff", fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, t)));
        meshRenderer.materials = mats;
    }
    void BornFVX()
    {
        Debug.Log("born");
        Material[] mats = meshRenderer.materials;
        t += Time.deltaTime;
        mats[0].SetFloat("_Cutoff", 0);
        
       // mats[0].SetFloat("_Cutoff", fadeOut.Evaluate(Mathf.InverseLerp(1, spawnEffectTime, t)));
        meshRenderer.materials = mats;
        Debug.Log("--------------------------------------------------------------------------------");
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
