using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MubHpData : MonoBehaviour
{
    ItemOnMob thisItemOnMob;

    [HideInInspector] public int MaxHp = 1000;
    [SerializeField] public int Hp = 1000;
    [SerializeField] private int currentHp;

    HealthBar Health;

    private void Awake()
    {
        thisItemOnMob = GetComponent<ItemOnMob>();
        Health = GetComponent<HealthBar>();
        MaxHp = thisItemOnMob.mobMaxHp;
        Health.Maxhealth = MaxHp;
        Hp = MaxHp;
        currentHp = Hp;
    }

    private void Start()
    {
        MaxHp = thisItemOnMob.mobMaxHp;
        Health.Maxhealth = MaxHp;
        Hp = MaxHp;
        currentHp = Hp;
    }

    private void Update()
    {
        HpCheck();
        Health.BarFilter();
    }


    /// <summary>
    /// 血量扣除
    /// </summary>
    /// <param name="Num">扣血量(請填入>=0整數)</param>
    public void HpDeduction(int Num)
    {
        Hp -= Num;
    }


    /// <summary>
    /// 檢查血量，並影響血量條
    /// </summary>
    public void HpCheck()
    {
        if (Hp != currentHp)
        {
            Debug.Log("扣血");
            currentHp = Hp;
        }
        if (currentHp < 0)
        {
            currentHp = 0;
            Hp = currentHp;
        }
    }
}