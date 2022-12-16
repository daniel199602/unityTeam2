using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpData : MonoBehaviour
{
    // Start is called before the first frame update
    ItemOnMob thisItemOnMob;

    [HideInInspector] public int MaxHp;
    [SerializeField] public int Hp;
    [SerializeField] private int currentHp;

    HealthPlayerBar Health;

    private void Awake()
    {
        thisItemOnMob = GetComponent<ItemOnMob>();
        Health = GetComponent<HealthPlayerBar>();
    }

    private void Start()
    {
        MaxHp = thisItemOnMob.mobMaxHp;
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
