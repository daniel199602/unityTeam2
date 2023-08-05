using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killMoBTest : MonoBehaviour
{
    MubHpData PlayerHp;

    private float duration;
    public GameObject mobDeathFVX;

    private void Awake()
    {
        PlayerHp = GetComponent<MubHpData>();
    }

    private void Update()
    {

    }
    public void killMobEvent_VFX()
    {
        ParticleSystem ps = mobDeathFVX.GetComponent<ParticleSystem>();
        ps.Play();
    }
    public void killMobEvent_VFX_Leave()
    {
        ParticleSystem ps = mobDeathFVX.GetComponent<ParticleSystem>();
        ps.Play();
        MobMain.Instance().oPool.UnLoadObjectToPool(MobMain.Instance().pTestList, this.gameObject);
        MobMain.Instance().pAliveObject.Remove(this.gameObject);
    }


    public void killMobEvent()
    {
        this.gameObject.transform.position = new Vector3(0,-200, 0);
        MobMain.Instance().oPool.UnLoadObjectToPool(MobMain.Instance().pTestList, this.gameObject);
        MobMain.Instance().pAliveObject.Remove(this.gameObject);
        int count = MobMain.Instance().pAliveObject.Count;

        if(count<=0)
        {
            MobMain.Instance().MobBox.SendMessage("born");
        }

    }
   

}
