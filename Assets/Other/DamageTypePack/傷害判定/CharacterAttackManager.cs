using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class CharacterAttackManager : MonoBehaviour
{
    
     private float angle=95f;
   
    private float radius=15f;

    [HideInInspector]public  int Weapondamage_Instant;

    [HideInInspector]public int Weapondamamge_Delay;

    [SerializeField] private GameObject Target;
    //[SerializeField] private GameObject[] Target;

    CapsuleCollider TargetSize;
    
    Weapon weaponData;
    
    public PlayerState PlayerData;
    
    private bool flag;
     
    PlayerGetHit playerGetHit;
    
    public int fHp;

    PlayerController playerController;
    public int cAMLayerNum;

    int DMtype=0;
    
    private void Start()
    {
        //for (int i = 0; i < Target.Length; i++)
        //{
        //    TargetSize= Target[i].GetComponent<CapsuleCollider>();
        //    PlayerData = Target[i].GetComponent<PlayerState>();
        //}
        TargetSize = Target.GetComponent<CapsuleCollider>();
        PlayerData = Target.GetComponent<PlayerState>();
        playerGetHit = GetComponent<PlayerGetHit>();
        weaponData = GetComponent<Weapon>();
        playerController = GetComponent<PlayerController>();
        cAMLayerNum = playerController.currentLayerNum;

        fHp = PlayerData.Hp;

        //radius= weaponData.weaPonRadius;
        //angle= weaponData.weaPonangle;
        //Weapondamage_Instant= weaponData.Weapon_Damage_Instant;
        //Weapondamamge_Delay= weaponData.Weapon_Damamge_Delay;
    }
    private void Update()
    {
        //cAMLayerNum = playerController.currentLayerNum;//不知道為什麼抓不到這個值_1130

        //if (cAMLayerNum==0)//Layer0
        //{
        //    weaponData.WeaponType(9);
        //    DMtype =1;
        //    radius = weaponData.weaPonRadius;
        //    angle = weaponData.weaPonangle;
        //    Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        //    Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //}
        //if(cAMLayerNum == 1)//Layer1
        //{
            //weaponData.WeaponType(6);
            //DMtype = 0;
            //radius = weaponData.weaPonRadius;
            //angle = weaponData.weaPonangle;
            //Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
            //Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //}
        //if(cAMLayerNum == 2)//Layer2
        //{
        //    weaponData.WeaponType(7);
        //    DMtype = 0;
        //    radius = weaponData.weaPonRadius;
        //    angle = weaponData.weaPonangle;
        //    Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        //    Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        //}


    }
    /// <summary>
    /// 動畫觸發的攻擊事件
    /// </summary>
    private void AttackEvent()
    {
        //for (int i = 0; i < Target.Length; i++)
        //{
        //    flag = IsInRange(angle, radius, gameObject.transform, Target[i].transform);
        //    if (flag == true)
        //    {
        //        playerGetHit.GetHitByOther(DMtype);
        //        PlayerData.Hp = fHp;
        //        Debug.LogWarning("Hit");
        //    }
        //}
        //
        flag = IsInRange(angle, radius, gameObject.transform, Target.transform);
        if (flag == true)
        {
            Debug.Log("打到了");
            playerGetHit.GetHitByOther(DMtype);
            PlayerData.Hp = fHp;
            Debug.LogWarning("Hit");
        }
    }
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        
        Vector3 direction = attacked.position - attacker.position;
       
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return offsetAngle < sectorAngle*.7f && direction.magnitude-TargetSize.radius< sectorRadius;
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