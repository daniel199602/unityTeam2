using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDeathVFX : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    public GameObject bearMaterial;
    public GameObject FireFVX;
    
    public float t = 0.0f;
    private void Start()
    {
        meshRenderer = bearMaterial.GetComponent<SkinnedMeshRenderer>();
    }
    void DeathFVXEvent()
    {
        Material[] mats = meshRenderer.materials;
        t += Time.deltaTime;
        mats[0].SetFloat("_Cutoff", Mathf.Lerp(0,1,t));
        
        meshRenderer.materials = mats;
       
        ParticleSystem ps = FireFVX.GetComponent<ParticleSystem>();
        ps.Play();
    }
    

}
