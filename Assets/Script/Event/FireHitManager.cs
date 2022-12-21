using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHitManager : MonoBehaviour
{
    [HideInInspector] public int mobDamage_instant;
    [HideInInspector] public int mobDamamge_delay;
    [SerializeField] private GameObject Target;
    PlayerHpData PlayerData;
    int TimeLine;
    // Start is called before the first frame update
    void Start()
    {
        TimeLine = 10;
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        Target = GameManager.Instance().PlayerStart;
        PlayerData = Target.GetComponent<PlayerHpData>();
        StartCoroutine(cc());
    }
    // Update is called once per frame    
    private void Update()
    {
        if (TimeLine <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        CancelInvoke("AttackEvent_Normal");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InvokeRepeating("AttackEvent_Normal",0, 1);            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CancelInvoke("AttackEvent_Normal");          
        }
    }
    private void AttackEvent_Normal()
    {       
        DeductMobHpInstant(Target, mobDamage_instant);
        DeductMobHpDelay(Target, mobDamamge_delay);
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
    IEnumerator cc()
    {
        yield return new WaitForSeconds(1);
        TimeLine--;
    }
}
