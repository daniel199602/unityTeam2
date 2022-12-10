using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBearMobTest : MonoBehaviour
{
    DoorOpen_MobLast doorOpen_MobLast;

   
    private void Start()
    {
        doorOpen_MobLast = this.gameObject.transform.parent.gameObject.GetComponent<DoorOpen_MobLast>();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        doorOpen_MobLast.killMob();
    //        //this.gameObject.SetActive(false);

    //    }
    //}
}
