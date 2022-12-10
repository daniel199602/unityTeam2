using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBearMobTest : MonoBehaviour
{
    DoorOpen_MobLast doorOpen_MobLast;
    PlayerState PlayerHp;
    private float duration;

    private void Awake()
    {
        PlayerHp = GetComponent<PlayerState>();
    }

    private void Start()
    {
        doorOpen_MobLast = this.gameObject.transform.parent.gameObject.GetComponent<DoorOpen_MobLast>();
    }

    private void Update()
    {

        if (PlayerHp.Hp <= 0)
        {
            killEliteMob();
        }
    }

    void killEliteMob()
    {
        doorOpen_MobLast.killMob();
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
