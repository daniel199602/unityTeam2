using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CharacterAttackManager : MonoBehaviour
{

    [HideInInspector] public float angle = 80f;

    [HideInInspector] public float radius = 80f;

    [HideInInspector] public int Weapondamage_Instant;

    [HideInInspector] public int Weapondamamge_Delay;

    [SerializeField] private List<GameObject> Target;

    [SerializeField] List<PlayerState> PlayerData;

    Weapon weaponData;

    private bool flag;

    PlayerGetHit TargetGetHit_DamageDeal;

    public int fHp;

    int DMtype = 0;


    private void Awake()
    {
        TargetGetHit_DamageDeal = GetComponent<PlayerGetHit>();
        weaponData = GetComponent<Weapon>();
    }

    private void Start()
    {
        fHp = 0;

        //����Ҧ��Ǫ�PlayerState���O�A�æs�i�hPlayerData
        foreach (GameObject mob in Target)
        {
            PlayerData.Add(mob.GetComponent<PlayerState>());
        }

        /*weaponData����_�Ȯɵ��Ѱ_��*/
        //radius = weaponData.weaPonRadius;
        //Debug.Log("radius" + radius);
        //angle = weaponData.weaPonangle;
        //Debug.Log(angle);
        //Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        //Debug.Log(Weapondamage_Instant);
        //Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //Debug.Log(Weapondamamge_Delay);
    }

    private void Update()
    {
        /*weaponData����_�Ȯɵ��Ѱ_��*/
        //weaponData.WeaponType(0);
        //radius = weaponData.weaPonRadius;
        //angle = weaponData.weaPonangle;
        //Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        //Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //DMtype = 0;
        Debug.Log("Target.Count:" + Target.Count);
        Debug.Log("PlayerData.Count:" + PlayerData.Count);
    }

    /// <summary>
    /// �j�b�����ʵe�ƥ�
    /// </summary>
    private void AttackEvent()
    {
        for (int i = 0; i <= Target.Count - 1; i++)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, Target[i].transform))
            {
                TargetGetHit_DamageDeal.GetHitByOther(DMtype);
                PlayerData[i].Hp += fHp;
                Debug.Log("�ڲq�����Ǫ�Hp" + PlayerData[i].Hp);
                Debug.Log("Hp���:" + (PlayerData[i].Hp += fHp));
                Debug.Log("�ˮ`�ƭ�:" + fHp);
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
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float xRadius = attacked.GetComponent<CapsuleCollider>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - xRadius < sectorRadius;
    }



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