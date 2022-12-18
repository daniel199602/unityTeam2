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
    GameObject Player;

    HealthPlayerBar Health;

    private void Awake()
    {
        thisItemOnMob = GetComponent<ItemOnMob>();
        Health = GetComponent<HealthPlayerBar>();
        MaxHp = thisItemOnMob.mobMaxHp;
    }

    private void Start()
    {
        Player = GameManager.Instance().PlayerStart;
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
        if (Player.GetComponent<PlayerController>().isInvincible == false)
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
}
