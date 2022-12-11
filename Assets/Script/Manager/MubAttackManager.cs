using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MubAttackManager : MonoBehaviour
{

    ItemOnMob thisItemOnMob;

    [HideInInspector] public float angle;

    [HideInInspector] public float radius;

    [HideInInspector] public int Weapondamage_Instant;

    [HideInInspector] public int Weapondamamge_Delay;

    [SerializeField] private GameObject Target;

    CapsuleCollider TargetSize;

    Weapon weaponData;

    PlayerState PlayerData;

    private bool flag;

    PlayerGetHit playerGetHit;

    public int fHp;

    int DMtype = 0;



    private void Start()
    {


        TargetSize = Target.GetComponent<CapsuleCollider>();
        PlayerData = Target.GetComponent<PlayerState>();

        playerGetHit = GetComponent<PlayerGetHit>();
        weaponData = GetComponent<Weapon>();


        fHp = 0;
        radius = weaponData.weaPonRadius;
        Debug.Log("radius" + radius);
        angle = weaponData.weaPonangle;
        Debug.Log(angle);
        Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        Debug.Log(Weapondamage_Instant);
        Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        Debug.Log(Weapondamamge_Delay);
    }
    private void Attack()
    {
        flag = IsInRange(angle, radius, gameObject.transform, Target.transform);
        if (flag == true)
        {
            playerGetHit.GetHitByOther(DMtype);
            PlayerData.Hp -= fHp;
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
    private void OnDrawGizmos()
    {
        Handles.color = flag ? Color.cyan : Color.red;

        float x = radius * Mathf.Sin(angle / 2f * Mathf.Deg2Rad);
        float z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));

        Vector3 a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
        Vector3 b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        Handles.DrawLine(transform.position, a);
        Handles.DrawLine(transform.position, b);

        float half = angle / 2;

        for (int i = 0; i < half; i++)
        {
            x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
            a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
            x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
            b = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + z);
            Handles.DrawLine(a, b);
        }

        for (int i = 0; i < half; i++)
        {
            x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
            a = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
            z = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
            b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            Handles.DrawLine(a, b);
        }
    }
}