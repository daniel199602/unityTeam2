using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpData : MonoBehaviour
{
    // Start is called before the first frame update
    ItemOnMob thisItemOnMob;

    [HideInInspector] public int MaxHp=4000;
    [SerializeField] public int Hp=4000;
    [SerializeField] private int currentHp=4000;
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
        MaxHp = thisItemOnMob.mobMaxHp;
        Hp = MaxHp;
        currentHp = Hp;
    }

    private void Update()
    {
        HpCheck();
        Health.BarFilter();
        if (Input.GetKey(KeyCode.F10))
        {
            Hp += 400;
        }
    }


    /// <summary>
    /// ��q����
    /// </summary>
    /// <param name="Num">����q(�ж�J>=0���)</param>
    public void HpDeduction(int Num)
    {
        if (GameManager.Instance().PlayerStart.GetComponent<PlayerController>().GetLayerNumNow() == 1)
        {
            Hp -= (int)(Num*0.9f);
        }
        else
        {
            Hp -= Num;
        }
       
        
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
                //Debug.Log("����");
                currentHp = Hp;
            }
            if (currentHp < 0)
            {
                currentHp = 0;
                Hp = 0;
            }
        }       
    }
}
