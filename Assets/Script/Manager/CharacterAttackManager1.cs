using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class CharacterAttackManager1 : MonoBehaviour
{

    [HideInInspector] public float angle = 80f;

    [HideInInspector] public float radius = 80f;

    [HideInInspector] public int Weapondamage_Instant;

    [HideInInspector] public int Weapondamamge_Delay;

    [SerializeField] private List<GameObject> Target;

    [SerializeField] private GameObject TargetFix;

    List<CapsuleCollider> TargetSize;

    CapsuleCollider TargetSizeFix;

    Weapon weaponData;

    [SerializeField] List<PlayerState> PlayerData;

    PlayerState PlayerDataFix;

    private bool flag;

    PlayerGetHit playerGetHit;

    public int fHp;

    int DMtype = 0;
 
    private void Awake()
    {
        //TargetSizeFix = TargetFix.GetComponent<CapsuleCollider>();
        //PlayerDataFix = TargetFix.GetComponent<PlayerState>();
        playerGetHit = GetComponent<PlayerGetHit>();
        weaponData = GetComponent<Weapon>();
        //PlayerData.Clear();
        //TargetSize.Clear();
    }
    private void Start()
    {        
        for (int i = 0; i <= Target.Count; i++)
        {
            Debug.Log(i);
            PlayerData = Target[i].GetComponents<PlayerState>().ToList();            
            TargetSize = Target[i].GetComponents<CapsuleCollider>().ToList();
        }

        // fHp = PlayerData.Hp;
        fHp = 0;

        /*weaponData相關_暫時註解起來*/
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
        /*weaponData相關_暫時註解起來*/
        //weaponData.WeaponType(0);
        //radius = weaponData.weaPonRadius;
        //angle = weaponData.weaPonangle;
        //Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        //Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //DMtype = 0;
        Debug.Log("Target.Count:" + Target.Count);
        Debug.Log("PlayerData.Count:" + PlayerData.Count);
        Debug.Log("TargetSize.Count:" + TargetSize.Count);
        Debug.Log("PlayerData.Hp00:" + PlayerData[0].Hp);
        Debug.Log("PlayerData.Hp01:" + PlayerData[1].Hp);
    }

    /// <summary>
    /// 1203_施工中_改宗倫寫的 AttackEvent()
    /// </summary>
    private void AttackEvent()
    {
        for (int i = 0; i <= Target.Count; i++)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, Target[i].transform,i))
            {
                fHp = 0;
                playerGetHit.GetHitByOther(DMtype);
                PlayerData[i].Hp -= fHp;
                Debug.Log("我猜有抓到怪物Hp" + PlayerData[i].Hp);
                Debug.LogWarning("Hit");
            }
        }        
        //if (IsInRange(angle, radius, gameObject.transform, TargetFix.transform))
        //{
        //    fHp = 0;
        //    playerGetHit.GetHitByOther(DMtype);
        //    PlayerDataFix.Hp -= fHp;
        //    //Debug.Log("我猜有抓到怪物Hp" + mob.GetComponent<PlayerState>().Hp);
        //    Debug.LogWarning("Hit");
        //}
    }
    /// <summary>
    /// 原本宗倫寫的 AttackEvent()
    /// </summary>
    //private void AttackEvent()
    //{
    //    for (int i = 0; i == Target.Count; i++)
    //    {
    //        flag = IsInRange(angle, radius, gameObject.transform, Target[i].transform, i);
    //        if (flag == true)
    //        {
    //            playerGetHit.GetHitByOther(DMtype);
    //            PlayerData[i].Hp -= fHp;
    //            Debug.LogWarning("Hit");
    //        }
    //    }
    //}


    /// <summary>
    /// 1203_施工中_改宗倫寫的 IsInRange()
    /// </summary>
    /// <param name="sectorAngle"></param>
    /// <param name="sectorRadius"></param>
    /// <param name="attacker"></param>
    /// <param name="attacked"></param>
    /// <returns></returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked,int x)
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        //float xRadius = attacked.GetComponent<CapsuleCollider>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - TargetSize[x].radius < sectorRadius;
    }
    /// <summary>
    /// 原本宗倫寫的 IsInRange()
    /// </summary>
    //public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked, int x)
    //{
    //    Vector3 direction = attacked.position - attacker.position;
    //    float dot = Vector3.Dot(direction.normalized, transform.forward);
    //    float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
    //    return offsetAngle < sectorAngle * .7f && direction.magnitude - TargetSize[x].radius < sectorRadius;
    //}


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