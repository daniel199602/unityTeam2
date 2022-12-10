using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class CharacterAttackManager : MonoBehaviour
{
    //之後寫武器管理器，看要直接寫在 CharacterAttackManager這，還是另外寫一個新的Class管理，目前先保留
    [HideInInspector] public float angle = 80f;
    [HideInInspector] public float radius = 80f;
    [HideInInspector] public int Weapondamage_Instant;
    [HideInInspector] public int Weapondamamge_Delay;

    Weapon weaponData;//想想怎了改，串接後刪除

    public int fHp;//牽扯到 PlayerGetHit 之後必刪掉，但先想想流程怎麼改、怎麼串接口

    int DMtype = 0;//牽扯到 PlayerGetHit 應該由 PlayerGetHit自己做事，之後刪
    int Type_weapon;//之後可能刪，想想怎麼接

    private void Awake()
    {
        weaponData = GetComponent<Weapon>();
        Type_weapon = weaponData.Weapon_Type;
    }

    private void Start()
    {
        fHp = 0;//牽扯到 PlayerGetHit 之後必刪掉

        /*1207 看Weapon數值測試用 之後武器管理寫好後會刪除*/
        Type_weapon = 2;
        weaponData.WeaponType(Type_weapon);
        /**/

        /*weaponData相關_暫時註解起來*/
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
        /*weaponData相關_暫時註解起來*/
        //1209之後武器管理員寫完後，必刪除
        weaponData.WeaponType(Type_weapon);
        radius = weaponData.weaPonRadius;
        angle = weaponData.weaPonangle;
        Weapondamage_Instant = weaponData.Weapon_Damage_Instant;
        Weapondamamge_Delay = weaponData.Weapon_Damamge_Delay;
        DMtype = 0;
    }

    /// <summary>
    /// 攻擊事件，綁在攻擊動畫上
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

                Debug.LogWarning("我猜有抓到怪物Hp" + mob.GetComponent<PlayerState>().Hp); //測試用之後刪
                Debug.LogWarning("Hp減少:" + (mob.GetComponent<PlayerState>().Hp += fHp)); //測試用之後刪
                Debug.LogWarning("傷害數值:" + fHp); //這個牽扯 GetHitByOther，必改，測試用之後刪
                Debug.LogWarning("Hit");
            }
        }
    }

    /// <summary>
    /// 攻擊範圍判定
    /// </summary>
    /// <param name="sectorAngle">角度</param>
    /// <param name="sectorRadius">距離</param>
    /// <param name="attacker">攻擊者</param>
    /// <param name="attacked">被攻擊者</param>
    /// <returns></returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)//1209稍微修整一下變數名稱
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float xRadius = attacked.GetComponent<CapsuleCollider>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - xRadius < sectorRadius;
    }

    /*1210 將原PlayerGetHit的傷害機制，合併寫進這裡*/
    //武器管理員寫好後，面對有 立即+延遲的傷害 要寫如何同時判斷1210
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
        Debug.Log("應造成傷害量:" + (fHp -= Weapondamage_Instant));
    }

    public void AttackWithDebuff()
    {
        fHp -= Weapondamage_Instant;
        StartCoroutine(DamageDelay());
        //Debug.Log("HP減少量" + fooHp.fHp);
        //Debug.Log("傷害量" + Weapondamage_Instant);
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


    //1209，OnDrawGizmos 這個先保留著好了
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