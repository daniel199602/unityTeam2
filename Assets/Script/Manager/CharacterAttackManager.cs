using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class CharacterAttackManager : MonoBehaviour
{
    //����g�Z���޲z���A�ݭn�����g�b CharacterAttackManager�o�A�٬O�t�~�g�@�ӷs��Class�޲z�A�ثe���O�d
    [HideInInspector] public float angle = 80f;
    [HideInInspector] public float radius = 80f;
    [HideInInspector] public int Weapondamage_Instant;
    [HideInInspector] public int Weapondamamge_Delay;

    Weapon weaponData;//�Q�Q��F��A�걵��R��

    public int fHp;//�o��� PlayerGetHit ���ᥲ�R���A�����Q�Q�y�{����B���걵�f

    int DMtype = 0;//�o��� PlayerGetHit ���ӥ� PlayerGetHit�ۤv���ơA����R
    int Type_weapon;//����i��R�A�Q�Q���

    private void Awake()
    {
        weaponData = GetComponent<Weapon>();
        Type_weapon = weaponData.Weapon_Type;
    }

    private void Start()
    {
        fHp = 0;//�o��� PlayerGetHit ���ᥲ�R��

        /*1207 ��Weapon�ƭȴ��ե� ����Z���޲z�g�n��|�R��*/
        Type_weapon = 2;
        weaponData.WeaponType(Type_weapon);
        /**/

        /*weaponData����_�Ȯɵ��Ѱ_��*/
        radius = weaponData.weaPonRadius;
        Debug.LogWarning("radius" + radius);
        angle = weaponData.weaPonangle;
        Debug.LogWarning(angle);
        Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        Debug.LogWarning(Weapondamage_Instant);
        Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        Debug.LogWarning(Weapondamamge_Delay);
    }

    private void Update()
    {
        /*weaponData����_�Ȯɵ��Ѱ_��*/
        //1209����Z���޲z���g����A���R��
        weaponData.WeaponType(Type_weapon);
        radius = weaponData.weaPonRadius;
        angle = weaponData.weaPonangle;
        Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        DMtype = 0;
    }

    /// <summary>
    /// �����ƥ�A�j�b�����ʵe�W
    /// </summary>
    private void AttackEvent()
    {
        foreach(GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                GetHitType_Damage(DMtype);

                mob.GetComponent<PlayerState>().HpDeduction(-fHp);

                Debug.LogWarning("�ڲq�����Ǫ�Hp" + mob.GetComponent<PlayerState>().Hp); //���եΤ���R
                Debug.LogWarning("Hp���:" + (mob.GetComponent<PlayerState>().Hp += fHp)); //���եΤ���R
                Debug.LogWarning("�ˮ`�ƭ�:" + fHp); //�o�Ӳo�� GetHitByOther�A����A���եΤ���R
                Debug.LogWarning("Hit");
            }
        }
    }

    /// <summary>
    /// �����d��P�w
    /// </summary>
    /// <param name="sectorAngle">����</param>
    /// <param name="sectorRadius">�Z��</param>
    /// <param name="attacker">������</param>
    /// <param name="attacked">�Q������</param>
    /// <returns></returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)//1209�y�L�׾�@�U�ܼƦW��
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float xRadius = attacked.GetComponent<CapsuleCollider>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - xRadius < sectorRadius;
    }

    /*1210 �N��PlayerGetHit���ˮ`����A�X�ּg�i�o��*/
    //�Z���޲z���g�n��A���靈 �ߧY+���𪺶ˮ` �n�g�p��P�ɧP�_1210
    public int GetHitType_Damage(int Type)
    {
        switch (Type)
        {
            case 0:
                AttackWithoutDebuff();
                break;
            case 1:
                AttackWithDebuff();
                break;
        }
        return Type;
    }

    public void AttackWithoutDebuff()
    {
        fHp = 0;
        fHp -= Weapondamage_Instant;
        Debug.Log("���y���ˮ`�q:" + (fHp -= Weapondamage_Instant));
    }

    public void AttackWithDebuff()
    {
        fHp -= Weapondamage_Instant;
        StartCoroutine(DamageDelay());
        //Debug.Log("HP��ֶq" + fooHp.fHp);
        //Debug.Log("�ˮ`�q" + Weapondamage_Instant);
    }


    IEnumerator DamageDelay()
    {
        int Count = 5;
        while (Count >= 0)
        {
            fHp = 0;
            yield return new WaitForSeconds(1);
            fHp -= Weapondamamge_Delay;
            Count--;
        }
    }
    /**/


    //1209�AOnDrawGizmos �o�ӥ��O�d�ۦn�F
    //private void OnDrawGizmos()
    //{
    //    Handles.color = flag ? Color.cyan : Color.red;

    //    float x = radius * Mathf.Sin(angle / 2f * Mathf.Deg2Rad);
    //    float z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));

    //    Vector3 a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
    //    Vector3 b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

    //    Handles.DrawLine(transform.position, a);
    //    Handles.DrawLine(transform.position, b);

    //    float half = angle / 2;

    //    for (int i = 0; i < half; i++)
    //    {
    //        x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
    //        z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
    //        x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
    //        z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        b = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
    //        Handles.DrawLine(a, b);
    //    }

    //    for (int i = 0; i < half; i++)
    //    {
    //        x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
    //        z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        a = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    //        x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
    //        z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    //        Handles.DrawLine(a, b);
    //    }
    //}
}