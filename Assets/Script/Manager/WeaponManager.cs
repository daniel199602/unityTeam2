using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    private GameObject player;//存玩家

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


    /*1213測試用，之後刪-------------------------------*/
    [HeaderAttribute("Test_WeaponSwitch( Press keyCode.L )")]
    public int test_type1_id = 10;
    [Range(2, 3)] public int test_type23 = 2;
    public int test_type2_id = 20;
    public int test_type3_id = 30;

    /// <summary>
    /// 測試用函式，專門用於武器切換用的測試
    /// </summary>
    private void Test_ChooseAndUseWeapon(int type1_id, int type2or3, int type2_id, int type3_id)
    {
        ChooseAndUseWeaponTest(1, type1_id);//右手_盾牌
        if (type2or3 == 2)
        {
            ChooseAndUseWeaponTest(2, type2_id);//左手_單手劍
        }
        else if (type2or3 == 3)
        {
            ChooseAndUseWeaponTest(3, type3_id);//左手_雙手劍
        }
    }
    /*-----------------------------------------------------*/

    private void Awake()
    {
        if (mInstance != null)
        {
            //Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            Debug.LogWarning("有兩個相同的singleton物件,WeaponManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);

        AddAllWeaponsInTheirWeaponPool();//將所有武器，分別加入他們各自的武器池
    }

    void Start()
    {
        player = GameManager.Instance().PlayerStart;//抓到玩家

        //玩家初始武器設定
        ChooseAndUseWeaponTest(0, 0);//初始火把

        //寫完三選一後，這邊就由三選一來設定，之後刪
        //ChooseAndUseWeaponTest(1, 10);//初始盾牌
        //ChooseAndUseWeaponTest(2, 20);//初始右手單手劍
        //ChooseAndUseWeaponTest(3, 31);//初始右手雙手劍
    }

    private void Update()
    {
        ////按L切換成測試inspector當前的type,id武器
        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    Test_ChooseAndUseWeapon(test_type1_id, test_type23, test_type2_id, test_type3_id);
        //}
    }

    /// <summary>
    /// 將所有武器，分別加入他們各自的武器池
    /// </summary>
    private void AddAllWeaponsInTheirWeaponPool()
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




    /// <summary>
    /// 選擇並使用該武器(不使用，專門測試用)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void ChooseAndUseWeaponTest(int type, int id)
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

    /*武器三選一*/
    //執行順位: 要在武器池抓完所有武器之後

    //第一步機制:
    //隨機從 weaponPoolR 抓3支不重複的右手武器(劍)
    //先抓1支雙手劍，再抓1支單手劍，再隨機抓1支右手劍，且如果有當前裝備的右手劍則要排除
    /// <summary>
    /// 從weaponPoolR中，取隨機不重複1支雙手劍、1支單手劍 、再1支右手劍
    /// 並回傳一個 List<GameObject>
    /// </summary>
    public List<GameObject> GetRandomThreeWeaponR()
    {
        int UsedWeaponType3Index = -1;//用過的雙手劍weaponPoolR 內的index
        int UsedWeaponType2Index = -1;//用過的單手劍weaponPoolR 內的index

        int randomIndex = UnityEngine.Random.Range(0, weaponPoolR.Count);
        List<GameObject> randomThreeWeaponRPool = new List<GameObject>();

        //雙手劍 && 不等於當前裝備右手武器
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (weaponPoolR[randomIndex].GetComponent<ItemOnWeapon>().weaponType == 3)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    UsedWeaponType3Index = randomIndex;
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        //單手劍 && 不等於當前裝備右手武器
        int randomIndexForSecondChoose = UnityEngine.Random.Range(0, weaponPoolR.Count);//增加隨機性
        randomIndex = randomIndexForSecondChoose;
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (weaponPoolR[randomIndex].GetComponent<ItemOnWeapon>().weaponType == 2)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    UsedWeaponType2Index = randomIndex;
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        //再隨機抓1支右手劍 && 不等於當前裝備右手武器 && 不重複前兩把武器
        int randomIndexForThridChoose = UnityEngine.Random.Range(0, weaponPoolR.Count);//增加隨機性
        randomIndex = randomIndexForThridChoose;
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (randomIndex != UsedWeaponType3Index && randomIndex != UsedWeaponType2Index)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        return randomThreeWeaponRPool;
    }

    //第二步機制:
    //隨機從 weaponPoolR 抓3支不重複的左手武器(盾)
    //如果有當前裝備的左手盾則要排除

    /// <summary>
    /// 從weaponPoolR中，取隨機不重複3把盾
    /// 並回傳一個 List<GameObject>
    /// </summary>
    public List<GameObject> GetRandomThreeWeaponL()
    {
        int UsedWeaponLIndexX = -1;
        int randomIndex = UnityEngine.Random.Range(0, weaponPoolL.Count);
        List<GameObject> randomThreeWeaponLPool = new List<GameObject>();

        //盾牌X
        for (int i = 0; i < weaponPoolL.Count; i++)
        {
            if (weaponPoolL[randomIndex] != CurrentWeaponL_weaponL)
            {
                randomThreeWeaponLPool.Add(weaponPoolL[randomIndex]);
                UsedWeaponLIndexX = randomIndex;
                break;
            }
            if (randomIndex == (weaponPoolL.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolL.Count) randomIndex++;
        }
        //盾牌Y
        for (int i = 0; i < weaponPoolL.Count + 2; i++)
        {
            if (randomIndex != UsedWeaponLIndexX)
            {
                if (weaponPoolL[randomIndex] != CurrentWeaponL_weaponL)
                {
                    randomThreeWeaponLPool.Add(weaponPoolL[randomIndex]);
                    break;
                }
            }
            if (randomIndex == (weaponPoolL.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolL.Count) randomIndex++;
        }
        //盾牌Z
        foreach (var weaponZ in weaponPoolL)
        {
            if (weaponZ != randomThreeWeaponLPool[0] && weaponZ != randomThreeWeaponLPool[1])
            {

                randomThreeWeaponLPool.Add(weaponZ);
                break;
            }
        }
        return randomThreeWeaponLPool;
    }

    /// <summary>
    /// 選擇並使用該武器
    /// (三選一機制會用到這個函式)
    /// </summary>
    public void ChooseAndUseWeapon(GameObject aWeapon)
    {
        int type = aWeapon.GetComponent<ItemOnWeapon>().weaponType;
        int id = aWeapon.GetComponent<ItemOnWeapon>().weaponID;

        WeaponSetActiveOpen(type, id);//開啟選擇的武器

        if (type == 0)
            this.CurrentTorchL_torch = GetWeapon(type, id);//當前火把
        if (type == 1)
            this.CurrentWeaponL_weaponL = GetWeapon(type, id);//當前左手武器
        UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//設置當前武器進武器格
        if (type == 2)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右單手武器
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//設置當前武器進武器格
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
        if (type == 3)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右雙手武器
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//設置當前武器進武器格
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
    }
    /*---------------------------------*/

    /// <summary>
    /// 清除玩家當前裝備的所有武器
    /// </summary>
    public void SetAllCurrentWeaponsEmpty()
    {
        CurrentTorchL_torch = null;
        CurrentWeaponL_weaponL = null;
        CurrentWeaponR_weaponR = null;
    }

}
