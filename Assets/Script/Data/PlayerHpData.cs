using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpData : MonoBehaviour
{
    // Start is called before the first frame update
    ItemOnMob thisItemOnMob;

    [HideInInspector] public int MaxHp = 1000;
    [SerializeField] public int Hp = 1000;
    [SerializeField] private int currentHp;

    HealthBar Health;

    private void Awake()
    {
        thisItemOnMob = GetComponent<ItemOnMob>();
        Health = GetComponent<HealthBar>();
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
    /// ��q����
    /// </summary>
    /// <param name="Num">����q(�ж�J>=0���)</param>
    public void HpDeduction(int Num)
    {
        Hp -= Num;
    }


    /// <summary>
    /// �ˬd��q�A�üv�T��q��
    /// </summary>
    public void HpCheck()
    {
        if (Hp != currentHp)
        {
            Debug.Log("����");
            currentHp = Hp;
        }
        if (currentHp < 0)
        {
            currentHp = 0;
            Hp = currentHp;
        }
    }
}