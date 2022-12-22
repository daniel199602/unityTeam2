using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class BossAttackManager : MonoBehaviour
{
    /*傷害存取站*/
    [HideInInspector] public int mobDamage_instant;
    [HideInInspector] public int mobDamage_instant_R;
    [HideInInspector] public int mobDamage_instant_U;
    [HideInInspector] public int mobDamamge_delay;
    [HideInInspector] public int mobDamamge_delay_R;
    [HideInInspector] public int mobDamamge_delay_U;
    [HideInInspector] public float mobAngle;
    [HideInInspector] public float mobAngle_R;
    [HideInInspector] public float mobAngle_U;
    [HideInInspector] public float mobRadius;
    [HideInInspector] public float mobRadius_R;
    [HideInInspector] public float mobRadius_U;

    //public GameObject hitVFX_Bear;

    //RecoilShake recoilShake;

    [SerializeField] private GameObject Target;//存玩家

    PlayerHpData PlayerData;//玩家血量狀態

    CharacterController TargetSize;//玩家用CharacterController

    private bool flag = false; //OnDrawGizmos判斷是否在範圍用

    Vector3 TargetN;//自己跟對方的向量

    int Hp;
    private void Start()
    {
        Target = GameManager.Instance().PlayerStart;
        PlayerData = Target.GetComponent<PlayerHpData>();
        Hp = PlayerData.Hp;

        /*抓出該怪物資料*/
        /*傷害*/
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamage_instant_R = GetComponent<ItemOnMob>().mobDamage_instant-30;
        mobDamage_instant_U = GetComponent<ItemOnMob>().mobDamage_instant +(Hp/2);
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        mobDamamge_delay_R = GetComponent<ItemOnMob>().mobDamage_delay+10;
        mobDamamge_delay_U = GetComponent<ItemOnMob>().mobDamage_delay*0;
        /*角度*/
        mobAngle = GetComponent<ItemOnMob>().mobAngle;
        mobAngle_R = GetComponent<ItemOnMob>().mobAngle / 3;
        mobAngle_U = GetComponent<ItemOnMob>().mobAngle / 5;
        /*距離*/
        mobRadius = GetComponent<ItemOnMob>().mobRadius;
        mobRadius_R = GetComponent<ItemOnMob>().mobRadius * 3f;
        mobRadius_U = GetComponent<ItemOnMob>().mobRadius * 30f;

        TargetSize = Target.GetComponent<CharacterController>();
        
        TargetN = (Target.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// 怪物攻擊事件
    /// </summary>
    private void AttackEvent_Normal()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (flag)
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);

            TargetSize.SimpleMove(TargetN * 20000 * Time.deltaTime);
        }
    }

    private void AttackEvent_Big()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (flag)
        {
            DeductMobHpInstant(Target, mobDamage_instant+10);
            DeductMobHpDelay(Target, mobDamamge_delay);

            TargetSize.SimpleMove(TargetN * 20000 * Time.deltaTime);
        }
    }

    private void AttackEvent_Ranged()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle_R, mobRadius_R, gameObject.transform, Target.transform);

        if (flag)
        {
            DeductMobHpInstant(Target, mobDamage_instant_R);
            DeductMobHpDelay(Target, mobDamamge_delay_R);

            TargetSize.SimpleMove(TargetN * 20000 * Time.deltaTime);
        }
    }

    private void AttackEvent_Ultimate()
    {
        //OnDrawGizmos判斷是否在範圍用
        flag = IsInRange(mobAngle_U, mobRadius_U, gameObject.transform, Target.transform);

        if (flag)
        {
            DeductMobHpInstant(Target, mobDamage_instant_U);
            DeductMobHpDelay(Target, mobDamamge_delay_U);
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
    private void OnDrawGizmos()
    {
        //右手武器攻擊半徑
        Gizmos.color = Color.red;
        float angleR = mobAngle;
        float radiusR = mobRadius;
        int segments = 100;
        float deltaAngle = angleR / segments;
        Vector3 forward = transform.forward;


        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = transform.position;
        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angleR / 2 + deltaAngle * (i - 1), 0f) * forward * radiusR + transform.position;
            vertices[i] = pos;
        }
        for (int i = 1; i < vertices.Length - 1; i++)
        {
            Gizmos.DrawLine(vertices[i], vertices[i + 1]);
        }
        Gizmos.DrawLine(vertices[0], vertices[vertices.Length - 1]);
        Gizmos.DrawLine(vertices[0], vertices[1]);

        //遠距離攻擊半徑
        Gizmos.color = Color.cyan;
        float angleR_R = mobAngle_R;
        float radiusR_R = mobRadius_R;
        int segments_R = 100;
        float deltaAngle_R = angleR_R / segments_R;
        Vector3 forward_R = transform.forward;


        Vector3[] vertices_R = new Vector3[segments_R + 2];
        vertices_R[0] = transform.position;
        for (int i = 1; i < vertices_R.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angleR_R / 2 + deltaAngle_R * (i - 1), 0f) * forward_R * radiusR_R + transform.position;
            vertices_R[i] = pos;
        }
        for (int i = 1; i < vertices_R.Length - 1; i++)
        {
            Gizmos.DrawLine(vertices_R[i], vertices_R[i + 1]);
        }
        Gizmos.DrawLine(vertices_R[0], vertices_R[vertices_R.Length - 1]);
        Gizmos.DrawLine(vertices_R[0], vertices_R[1]);

        //遠距離攻擊半徑
        Gizmos.color = Color.cyan;
        float angleR_u = mobAngle_U;
        float radiusR_u = mobRadius_U;
        int segments_u = 100;
        float deltaAngle_u = angleR_u / segments_u;
        Vector3 forward_u = transform.forward;


        Vector3[] vertices_u = new Vector3[segments_u + 2];
        vertices_u[0] = transform.position;
        for (int i = 1; i < vertices_R.Length; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, -angleR_u / 2 + deltaAngle_u * (i - 1), 0f) * forward_u * radiusR_u + transform.position;
            vertices_u[i] = pos;
        }
        for (int i = 1; i < vertices_u.Length - 1; i++)
        {
            Gizmos.DrawLine(vertices_u[i], vertices_u[i + 1]);
        }
        Gizmos.DrawLine(vertices_u[0], vertices_u[vertices_u.Length - 1]);
        Gizmos.DrawLine(vertices_u[0], vertices_u[1]);
    }
}