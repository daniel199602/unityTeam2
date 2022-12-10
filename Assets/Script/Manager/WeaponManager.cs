using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    /*抓出所有武器的數值存在這*/
    public GameObject torchL;//綁火的左手
    public GameObject weaponL;//左手
    public GameObject weaponR;//右手

    public List<GameObject> torchPoolL;
    public List<GameObject> weaponPoolL;
    public List<GameObject> weaponPoolR;

    /*設一個接口，控制所有武器的開啟關閉(除了火把)。SetActive*/
    public void WeaponSetActiveOpen(int id)
    {
        foreach (GameObject weapon in weaponPoolL)
        {
            if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
            {
                weapon.SetActive(true);
                //int damage_instant = weapon.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                //int damage_delay = weapon.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                //float angle = weapon.GetComponent<ItemOnWeapon>().weaponAngle;
                //float radius = weapon.GetComponent<ItemOnWeapon>().weaponRadius;
            }
        }
    }

    /*設一個接口，抓出該武器，傳給CharacterAttackManager或傳給GameManager*/
    /// <summary>
    /// 獲取武器數值
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetWeaponNum(int type,int id)
    {
        if (type == 0)//左單手火把_0
        {
            foreach (GameObject weapon in torchPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if (type == 1)//左單手盾_1
        {
            foreach (GameObject weapon in weaponPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if (type == 2||type == 3)//右單手劍_2 or 右雙手劍_3
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        
        return null;
    }

    private void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        //左手火把存進 火把池(L)
        if (torchL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < torchL.transform.childCount; i++)
            {
                torchPoolL.Add(torchL.transform.GetChild(i).gameObject);
            }
        }
        //左手武器存進 武器池(L)
        if (weaponL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponL.transform.childCount; i++)
            {
                weaponPoolL.Add(weaponL.transform.GetChild(i).gameObject);
            }
        }
        //右手武器存進 武器池(R)
        if (weaponR.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponR.transform.childCount; i++)
            {
                weaponPoolR.Add(weaponR.transform.GetChild(i).gameObject);
            }
        }
    }

    void Update()
    {
        
    }
}
