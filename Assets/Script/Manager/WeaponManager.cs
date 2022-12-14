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

    /*非常非常重要的設定!!_會影響PlayerController的動畫Layer的切換*/
    //左單火把_type 0, id 範圍 0~9 整數
    //左單手盾_type 1, id 範圍 10~19 整數
    //右單手劍_type 2, id 範圍 20~29 整數
    //右雙手劍_type 3, id 範圍 30~39 整數
    /**/

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
    /// PlayerController判斷玩家當前Animator Layer的依據，右手武器type==2 or type==3
    /// </summary>
    public GameObject CurrentWeaponR_weaponR { private set; get; }

    GameObject player;//存玩家

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);

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

        player = GameManager.Instance().PlayerStart;//抓到玩家
    }

    void Start()
    {
        //玩家初始武器設定
        ChooseAndUseWeapon(0, 0);//初始火把
        ChooseAndUseWeapon(1, 10);//初始盾牌
        ChooseAndUseWeapon(2, 20);//初始右手單手劍
    }


    /*1213測試用，之後刪-------------------------------*/
    [HeaderAttribute("Test_WeaponSwitch")]
    public int test_type1_id = 10;
    [Range(2, 3)] public int test_type23 = 2;
    public int test_type2_id = 20;
    public int test_type3_id = 30;
    /// <summary>
    /// 測試用函式，專門用於武器切換用的測試
    /// </summary>
    private void Test_ChooseAndUseWeapon(int type1_id, int type2or3, int type2_id,int type3_id)
    {
        
        ChooseAndUseWeapon(1, type1_id);//右手_盾牌
        if (type2or3 == 2)
        {
            ChooseAndUseWeapon(2, type2_id);//左手_單手劍
        }
        else if(type2or3 == 3)
        {
            ChooseAndUseWeapon(3, type3_id);//左手_雙手劍
        }

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Test_ChooseAndUseWeapon(test_type1_id, test_type23, test_type2_id, test_type3_id);
        }
    }
    /*-------------------------------*/


    /// <summary>
    /// 選擇並使用該武器
    /// (三選一機制會用到這個函式)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void ChooseAndUseWeapon(int type, int id)
    {
        WeaponSetActiveOpen(type, id);//開啟選擇的武器

        if (type == 0)
            this.CurrentTorchL_torch = GetWeapon(type, id);//當前火把
        if (type == 1)
            this.CurrentWeaponL_weaponL = GetWeapon(type, id);//當前左手武器
        if (type == 2)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右單手武器
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
        if (type == 3)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右雙手武器
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
    }

    /// <summary>
    /// 開啟選擇的武器，並關閉其他同類型的武器
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id">武器id</param>
    public void WeaponSetActiveOpen(int type, int id)
    {
        if (type == 0)//左單火把_type0
        {
            foreach (GameObject weapon in torchPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("火把的type:" + type+"id:"+id);//debug
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
                    Debug.LogWarning("左單手盾type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 2)//右單手劍_type2
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("右單手劍 type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 3)//右雙手劍_type3
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("右雙手劍 type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
    }


    /// <summary>
    /// 抓出該武器物件(不會切換)
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id">武器id</param>
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
        if (type == 2)//右單手劍_2
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if(type == 3)//右雙手劍_3
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
