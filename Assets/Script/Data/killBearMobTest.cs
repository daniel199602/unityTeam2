using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBearMobTest : MonoBehaviour
{
    DoorOpen_MobLast doorOpen_MobLast;
    MubHpData PlayerHp;
    private float duration;
    bool killThis_doorAmountReduced;

    private void Awake()
    {
        PlayerHp = GetComponent<MubHpData>();
    }

    private void Start()
    {
        doorOpen_MobLast = this.gameObject.transform.parent.gameObject.GetComponent<DoorOpen_MobLast>();
        killThis_doorAmountReduced = false;
    }

    private void Update()
    {
        if(PlayerHp.Hp==0 && killThis_doorAmountReduced == false)
        {
            doorOpen_MobLast.killMob();
            killThis_doorAmountReduced = true;
        }

    }

    void killEliteMobEvent()
    {
        this.gameObject.transform.position = new Vector3(0, -200, 0);
        this.gameObject.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        doorOpen_MobLast.killMob();
    //        this.gameObject.SetActive(false);

    //    }
    //}
}
