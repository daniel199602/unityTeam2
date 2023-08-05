﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class MubAttackManager : MonoBehaviour
{
    [HideInInspector] public int mobDamage_instant;
    [HideInInspector] public int mobDamamge_delay;
    [HideInInspector] public float mobAngle;
    [HideInInspector] public float mobRadius;
    //法師擊中特效
    public GameObject HitEffect;

    public GameObject hitVFX_Bear;

    RecoilShake recoilShake;

    public GameObject HitEffect_C;

    public GameObject hitVFX_Bear_C;

    [SerializeField] private GameObject Target;//存玩家
    GameObject ParticleSpace;

    PlayerHpData PlayerData;//玩家血量狀態
    CharacterController TargetSize;//玩家用CharacterController

    private bool flag = false; //OnDrawGizmos判斷是否在範圍用
    int fHp = 0; //1211目前PlayerGetHit有用到它刪掉會報錯，然後怪物有用PlayerGetHit，所以PlayerGetHit目前還不能刪
    Vector3 TargetN;//自己跟對方的向量

    private void Start()
    {
        Target = GameManager.Instance().PlayerStart;
        recoilShake = GetComponent<RecoilShake>();

        /*1211抓出該怪物資料*/
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        mobAngle = GetComponent<ItemOnMob>().mobAngle;
        mobRadius = GetComponent<ItemOnMob>().mobRadius;
        /**/
        ParticleSpace = Target.transform.GetChild(0).gameObject;
        // Debug.Log(ParticleSpace);
        TargetSize = Target.GetComponent<CharacterController>();
        PlayerData = Target.GetComponent<PlayerHpData>();
        TargetN = (Target.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// 怪物攻擊事件
    /// </summary>
    private void AttackEvent()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform))
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);

            Debug.LogWarning("Hit");
        }

    }
    private void AttackEvent_Crystal()
    {
        //OnDrawGizmos判斷是否在範圍用
        DeductMobHpInstant(Target, mobDamage_instant);
        DeductMobHpDelay(Target, mobDamamge_delay);
        Vector3 Pp = new Vector3(ParticleSpace.transform.position.x, ParticleSpace.transform.position.y + 5, ParticleSpace.transform.position.z);
        Instantiate(HitEffect_C, Pp, ParticleSpace.transform.rotation, ParticleSpace.transform);
        ParticleSystem ps = hitVFX_Bear_C.GetComponent<ParticleSystem>();
        Instantiate(ps, Target.transform.position, Target.transform.rotation);
        ps.Play();
        Debug.LogWarning("Hit");
    }
    private void AttackEvent_MagicCaster()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform))
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);
            Vector3 Pp = new Vector3(ParticleSpace.transform.position.x, ParticleSpace.transform.position.y + 5, ParticleSpace.transform.position.z);
            Instantiate(HitEffect, Pp, ParticleSpace.transform.rotation, ParticleSpace.transform);
            Debug.LogWarning("Hit");
        }

    }
    /// <summary>
    /// 怪物攻擊事件_Bear用
    /// </summary>
    private void AttackEvent_Bear()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform))
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);
            ParticleSystem ps = hitVFX_Bear.GetComponent<ParticleSystem>();
            Vector3 Pp = new Vector3(ParticleSpace.transform.position.x, ParticleSpace.transform.position.y + 5, ParticleSpace.transform.position.z);
            Instantiate(ps, Pp, Target.transform.rotation, ParticleSpace.transform);
            ps.Play();
            recoilShake.camraBearSake();
            Debug.LogWarning("Hit");
        }

    }
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        Vector3 direction = attacked.position - attacker.position;

        float dot = Vector3.Dot(direction.normalized, transform.forward);

        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return offsetAngle < sectorAngle * .7f && direction.magnitude - TargetSize.radius < sectorRadius;
    }


    /// <summary>
    /// 扣怪物血_只算立即傷害
    /// </summary>
    /// <param name="mob">怪物</param>
    /// <param name="demage_instant">立即傷害</param>
    public void DeductMobHpInstant(GameObject mob, int demage_instant)
    {
        mob.GetComponent<PlayerHpData>().HpDeduction(demage_instant);
    }


    /// <summary>
    /// 扣怪物血_只算Debuff造成延遲傷害
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    public void DeductMobHpDelay(GameObject mob, int demage_delay)
    {
        StartCoroutine(DamageDelay(mob, demage_delay));
        Debug.LogWarning("持續扣血結束");
    }
    /// <summary>
    /// 延遲傷害_Debuff持續扣血
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    /// <returns></returns>
    IEnumerator DamageDelay(GameObject mob, int demage_delay)
    {
        int Count = 5;
        while (Count >= 0)
        {
            yield return new WaitForSeconds(1);
            mob.GetComponent<PlayerHpData>().HpDeduction(demage_delay);
            Count--;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    //右手武器攻擊半徑
    //    Gizmos.color = flag? Color.white: Color.black;
    //    float angleR = mobAngle;
    //    float radiusR = mobRadius;
    //    int segments = 100;
    //    float deltaAngle = angleR / segments;
    //    Vector3 forward = transform.forward;


    //    Vector3[] vertices = new Vector3[segments + 2];
    //    vertices[0] = transform.position;
    //    for (int i = 1; i < vertices.Length; i++)
    //    {
    //        Vector3 pos = Quaternion.Euler(0f, -angleR / 2 + deltaAngle * (i - 1), 0f) * forward * radiusR + transform.position;
    //        vertices[i] = pos;
    //    }
    //    for (int i = 1; i < vertices.Length - 1; i++)
    //    {
    //        Gizmos.DrawLine(vertices[i], vertices[i + 1]);
    //    }
    //    Gizmos.DrawLine(vertices[0], vertices[vertices.Length - 1]);
    //    Gizmos.DrawLine(vertices[0], vertices[1]);

    //}
}