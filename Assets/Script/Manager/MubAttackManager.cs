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

    PlayerHpData PlayerData;//���a��q���A
    CharacterController TargetSize;//���a��CharacterController

    //Weapon weaponData;


    //PlayerGetHit playerGetHit;
    private bool flag = false; //OnDrawGizmos�P�_�O�_�b�d���
    public int fHp = 0; //1211�ثePlayerGetHit���Ψ쥦�R���|�����A�M��Ǫ�����PlayerGetHit�A�ҥHPlayerGetHit�ثe�٤���R
    Vector3 TargetN;//�ۤv���誺�V�q
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
        PlayerData = Target.GetComponent<PlayerHpData>();
        TargetN = (Target.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// �Ǫ������ƥ�
    /// </summary>
    private void AttackEvent()
    {
        //OnDrawGizmos�P�_�O�_�b�d���
        flag = IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform);

        if (IsInRange(mobAngle, mobRadius, gameObject.transform, Target.transform))
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);

            TargetSize.SimpleMove(TargetN * 500);
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
        mob.GetComponent<PlayerHpData>().HpDeduction(demage_instant);
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
            mob.GetComponent<PlayerHpData>().HpDeduction(demage_delay);
            Count--;
        }
    }





    private void OnDrawGizmos()
    {
        //�k��Z�������b�|
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

    }
}