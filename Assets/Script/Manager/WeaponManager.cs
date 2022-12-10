using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    public GameObject torchL;//綁火把的左手
    public GameObject weaponL;//左手
    public GameObject weaponR;//右手

    public List<GameObject> torchPoolL;//火把池
    public List<GameObject> weaponPoolL;//左手武器池
    public List<GameObject> weaponPoolR;//右手武器池

    /// <summary>
    /// 當前使用中的火把
    /// </summary>
    public GameObject CurrentTorchL_torch { private set; get; }
    /// <summary>
    /// 當前使用中的左手武器
    /// </summary>
    public GameObject CurrentWeaponL_weaponL { private set; get; }
    /// <summary>
    /// 當前使用中的右手武器
    /// </summary>
    public GameObject CurrentWeaponR_weaponR { private set; get; }

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

        //玩家初始武器設定
        ChooseAndUseWeapon(0, 0);//初始火把
        ChooseAndUseWeapon(1, 0);//初始盾牌
        ChooseAndUseWeapon(2, 0);//初始右手單手劍
    }

    /// <summary>
    /// 選擇並使用該武器
    /// (三選一機制會用到這個函式)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public GameObject ChooseAndUseWeapon(int type, int id)
    {
        WeaponSetActiveOpen(type, id);//開啟選擇的武器

        if (type == 0) this.CurrentTorchL_torch = GetWeapon(type, id);//當前火把
        if (type == 1) this.CurrentWeaponL_weaponL = GetWeapon(type, id);//當前左手武器
        if (type == 2 || type == 3) this.CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右手武器
        return GetWeapon(type, id);//抓出該武器資料
    }

    /// <summary>
    /// 控制所有武器的開啟關閉
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void WeaponSetActiveOpen(int type, int id)
    {
        if (type == 0)//左單火把_type0
        {
            foreach (GameObject weapon in torchPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 1)//左單手盾_type1
        {
            foreach (GameObject weapon in weaponPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 2 || type == 3)//右單手劍_type2 or 右雙手劍_type3
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
    }


    /// <summary>
    /// 抓出該武器物件
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetWeapon(int type, int id)
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
        if (type == 2 || type == 3)//右單手劍_2 or 右雙手劍_3
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


}
