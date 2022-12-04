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
         問題:IF玩家提前碰到法球       
         解法1:拋物線
         解法2:實時偵測玩家位置
        */
    }
    public void LongRangeAttack_FireRay()
    {
        var direction = Self.transform.position - AttackTarget.transform.position;
        Self.transform.forward = direction;
        
        /*
         問題:IF玩家提前碰到法球       
         解法1:拋物線
         解法2:實時偵測玩家位置
        */
    }
    public void MeleeAttack()
    {
        var direction = Self.transform.position - AttackTarget.transform.position;
        Self.transform.forward = direction;
       //Animation播放攻擊，EVENT判斷攻擊命中        
    }
}
