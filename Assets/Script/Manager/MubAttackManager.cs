using UnityEngine;
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
    
    [SerializeField] private GameObject Target;//存玩家

    PlayerState PlayerData;//玩家血量狀態
    CharacterController TargetSize;//玩家用CharacterController

    //Weapon weaponData;


    //PlayerGetHit playerGetHit;
    private bool flag = false; //OnDrawGizmos判斷是否在範圍用
    public int fHp = 0; //1211目前PlayerGetHit有用到它刪掉會報錯，然後怪物有用PlayerGetHit，所以PlayerGetHit目前還不能刪

    //int DMtype = 0;



    private void Start()
    {
        Target = GameManager.Instance().PlayerStart;

        /*1211抓出該怪物資料*/
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        mobAngle = GetComponent<ItemOnMob>().mobAngle;
        mobRadius = GetComponent<ItemOnMob>().mobRadius;
        /**/

        TargetSize = Target.GetComponent<CharacterController>();
        PlayerData = Target.GetComponent<PlayerState>();
    }

    /// <summary>
    /// 怪物攻擊事件
    /// </summary>
    private void Attack()
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
        mob.GetComponent<PlayerState>().HpDeduction(demage_instant);
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
            mob.GetComponent<PlayerState>().HpDeduction(demage_delay);
            Count--;
        }
    }





    private void OnDrawGizmos()
    {
        Handles.color = flag ? Color.cyan : Color.red;

        float x = mobRadius * Mathf.Sin(mobAngle / 2f * Mathf.Deg2Rad);
        float z = Mathf.Sqrt(Mathf.Pow(mobRadius, 2f) - Mathf.Pow(x, 2f));

        Vector3 a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
        Vector3 b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        Handles.DrawLine(transform.position, a);
        Handles.DrawLine(transform.position, b);

        float half = mobAngle / 2;

        for (int i = 0; i < half; i++)
        {
            x = mobRadius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(mobRadius, 2f) - Mathf.Pow(x, 2f));
            a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
            x = mobRadius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(mobRadius, 2f) - Mathf.Pow(x, 2f));
            b = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
            Handles.DrawLine(a, b);
        }

        for (int i = 0; i < half; i++)
        {
            x = mobRadius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(mobRadius, 2f) - Mathf.Pow(x, 2f));
            a = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            x = mobRadius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(mobRadius, 2f) - Mathf.Pow(x, 2f));
            b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            Handles.DrawLine(a, b);
        }
    }
}