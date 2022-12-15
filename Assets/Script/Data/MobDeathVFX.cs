using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDeathVFX : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    public GameObject bearMaterial;
    public GameObject FireFVX;
    public AnimationCurve fadeIn;
    public float timer = 0.0f;
    public float spawnEffectTime = 2;

    private void Start()
    {
        meshRenderer = bearMaterial.GetComponent<SkinnedMeshRenderer>();
    }
    void DeathFVXEvent()
    {
        Material[] mats = meshRenderer.materials;
        timer += Time.deltaTime;
        mats[0].SetFloat("_Cutoff", fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
        
        meshRenderer.materials = mats;
       
        ParticleSystem ps = FireFVX.GetComponent<ParticleSystem>();
        ps.Play();
    }
    

}
