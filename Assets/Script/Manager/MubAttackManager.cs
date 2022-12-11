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
    
    [SerializeField] private GameObject Target;//�s���a

    PlayerState PlayerData;//���a��q���A
    CharacterController TargetSize;//���a��CharacterController

    //Weapon weaponData;


    //PlayerGetHit playerGetHit;
    private bool flag = false; //OnDrawGizmos�P�_�O�_�b�d���
    public int fHp = 0; //1211�ثePlayerGetHit���Ψ쥦�R���|�����A�M��Ǫ�����PlayerGetHit�A�ҥHPlayerGetHit�ثe�٤���R

    //int DMtype = 0;



    private void Start()
    {
        Target = GameManager.Instance().PlayerStart;

        /*1211��X�өǪ����*/
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        mobAngle = GetComponent<ItemOnMob>().mobAngle;
        mobRadius = GetComponent<ItemOnMob>().mobRadius;
        /**/

        TargetSize = Target.GetComponent<CharacterController>();
        PlayerData = Target.GetComponent<PlayerState>();
    }

    /// <summary>
    /// �Ǫ������ƥ�
    /// </summary>
    private void Attack()
    {
        //OnDrawGizmos�P�_�O�_�b�d���
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
    /// ���Ǫ���_�u��ߧY�ˮ`
    /// </summary>
    /// <param name="mob">�Ǫ�</param>
    /// <param name="demage_instant">�ߧY�ˮ`</param>
    public void DeductMobHpInstant(GameObject mob, int demage_instant)
    {
        mob.GetComponent<PlayerState>().HpDeduction(demage_instant);
    }


    /// <summary>
    /// ���Ǫ���_�u��Debuff�y������ˮ`
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    public void DeductMobHpDelay(GameObject mob, int demage_delay)
    {
        StartCoroutine(DamageDelay(mob, demage_delay));
        Debug.LogWarning("���򦩦嵲��");
    }
    /// <summary>
    /// ����ˮ`_Debuff���򦩦�
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