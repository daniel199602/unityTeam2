using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFireWork : MonoBehaviour
{
    public GameObject bossHp;
    MubHpData mubHpData;
    
    public GameObject fireWork_VFX;
    public GameObject fireWork_VFX_2;
    public GameObject fireWork_VFX_3;
    public GameObject fireWork_VFX_4;

    ParticleSystem ps;
    ParticleSystem ps2;
    ParticleSystem ps3;
    ParticleSystem ps4;


    bool isDoItOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        mubHpData = bossHp.GetComponent<MubHpData>();
        ps = fireWork_VFX.GetComponent<ParticleSystem>();
        ps2 = fireWork_VFX_2.GetComponent<ParticleSystem>();
        ps3 = fireWork_VFX_3.GetComponent<ParticleSystem>();
        ps4 = fireWork_VFX_4.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mubHpData.Hp <= 0 && isDoItOnce)
        {
            ps.Play();
            ps2.Play();
            ps3.Play();
            ps4.Play();
            StartCoroutine(FireWorkDelayClose());
            isDoItOnce = false;
        }
    }

    IEnumerator FireWorkDelayClose()
    {
        yield return new WaitForSeconds(2);
        ps.Play();
        ps2.Play();
        ps3.Play();
        ps4.Play();
    }
}
