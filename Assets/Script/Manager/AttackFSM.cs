using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFSM :MonoBehaviour
{
    public GameObject Bullet;
    private GameObject AttackTarget;
    private GameObject Self;
   public void LongRangeAttack()
    {
        var direction = Self.transform.position - AttackTarget.transform.position;
        Self.transform.forward = direction;
        GameObject BulletOT;
        BulletOT=Instantiate(Bullet, transform.position, transform.rotation)as GameObject;
        BulletOT.transform.position = Vector3.MoveTowards(transform.position, direction, Time.deltaTime);
        /*
         ���D:IF���a���e�I��k�y       
         �Ѫk1:�ߪ��u
         �Ѫk2:��ɰ������a��m
        */
    }
    
}
