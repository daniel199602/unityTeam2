using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpData : MonoBehaviour
{
    private static PlayerHpData mInstance;
    public static PlayerHpData Instance() { return mInstance; }

    ItemOnMob thisItemOnMob;
    [HideInInspector] public int MaxHp=10000;
    [SerializeField] public int Hp=10000;
    [SerializeField] private int currentHp=10000;
    PlayerController playerController;



    private void Awake()
    {
        mInstance = this;
        thisItemOnMob = GetComponent<ItemOnMob>();
        playerController = GetComponent<PlayerController>();
        MaxHp = thisItemOnMob.mobMaxHp;
        
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

        if (Input.GetKey(KeyCode.F10))
        {
            Hp += 400;
        }
    }


    /// <summary>
    /// 血量扣除
    /// </summary>
    /// <param name="Num">扣血量(請填入>=0整數)</param>
    public void HpDeduction(int Num)
    {
        if (playerController.GetLayerNumNow() == 1)
        {
            Hp -= (int)(Num*0.9f);
        }
        else
        {
            Hp -= Num;
        }
    }


    /// <summary>
    /// 檢查血量，並影響血量條
    /// </summary>
    public void HpCheck()
    {
        if (playerController.isInvincible == false)
        {
            if (Hp != currentHp)
            {
                //Debug.Log("扣血");
                currentHp = Hp;
            }
            if (currentHp < 0)
            {
                currentHp = 0;
                Hp = 0;
            }
            if (currentHp > MaxHp)
            {
                currentHp = MaxHp;
            }
        }       
    }
}
