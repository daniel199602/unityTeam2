using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killMoBTest : MonoBehaviour
{
    PlayerState PlayerHp;

    private float duration;

    private void Awake()
    {
        PlayerHp = GetComponent<PlayerState>();
    }

    private void Update()
    {
        
        if (PlayerHp.Hp<=0)
        {
            killMob();
        }
    }

    
    public void killMob()
    {
        
        MobMain.Instance().oPool.UnLoadObjectToPool(MobMain.Instance().pTestList, this.gameObject);
        MobMain.Instance().pAliveObject.Remove(this.gameObject);
        int count = MobMain.Instance().pAliveObject.Count;

        if(count<=0)
        {
            MobMain.Instance().MobBox.SendMessage("born");
        }

    }
   

}
