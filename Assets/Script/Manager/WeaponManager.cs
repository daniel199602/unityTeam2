using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 武器管理員
/// </summary>
public class WeaponManager : Singleton<WeaponManager>
{
    PlayerController playerController;
    GameObject torchL = null;//綁火把的左手
    GameObject weaponL = null;//左手
    GameObject weaponR = null;//右手

    List<ItemOnWeapon> torchPoolL = new List<ItemOnWeapon>();//火把池
    List<ItemOnWeapon> weaponPoolL = new List<ItemOnWeapon>();//左手武器池
    List<ItemOnWeapon> weaponPoolR = new List<ItemOnWeapon>();//右手武器池

    /*非常非常重要的設定!!_會影響PlayerController的動畫Layer的切換*/
    //左單火把_type 0, id 範圍 0~9 整數
    //左單手盾_type 1, id 範圍 10~19 整數
    //右單手劍_type 2, id 範圍 20~29 整數
    //右雙手劍_type 3, id 範圍 30~39 整數
    /**/

    //武器切換後UI應該要被通知並切換圖示


    /// <summary>
    /// 當前使用中的火把
    /// </summary>
    public ItemOnWeapon CurrentTorchL_torch { private set; get; }
    /// <summary>
    /// 當前使用中的左手武器
    /// </summary>
    public ItemOnWeapon CurrentWeaponL_weaponL { private set; get; }
    /// <summary>
    /// 當前使用中的右手武器
    /// PlayerController判斷玩家當前Animator Layer的依據
    /// </summary>
    public ItemOnWeapon CurrentWeaponR_weaponR { private set; get; }

    /// <summary>
    /// 初始化
    /// 統一讓GameManager初始化
    /// </summary>
    /// <param name="player">玩家</param>
    public void Initialize(GameObject player)
    {
        this.playerController = player.GetComponent<PlayerController>();
        torchL = playerController.torchL;//綁火把的左手物件
        weaponL = playerController.weaponL;//左手武器物件
        weaponR = playerController.weaponR;//右手武器物件

        //將所有武器，分別加入他們各自的武器池
        AddAllWeaponsInTheirWeaponPool();

        //玩家初始武器設定
        EquipWeapon(WeaponType.LeftTorch, 0);//初始火把

        //武器設定示範
        //ChooseAndUseWeaponTest(1, 10);//初始盾牌
        //ChooseAndUseWeaponTest(2, 20);//初始右手單手劍
        //ChooseAndUseWeaponTest(3, 31);//初始右手雙手劍
    }

    /// <summary>
    /// 將所有武器，分別加入他們各自的武器池
    /// </summary>
    void AddAllWeaponsInTheirWeaponPool()
    {
        //左手火把存進 火把池(L)
        if (torchL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < torchL.transform.childCount; i++)
            {
                torchPoolL.Add(torchL.transform.GetChild(i).GetComponent<ItemOnWeapon>());
            }
        }
        //左手武器存進 武器池(L)
        if (weaponL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponL.transform.childCount; i++)
            {
                weaponPoolL.Add(weaponL.transform.GetChild(i).GetComponent<ItemOnWeapon>());
            }
        }
        //右手武器存進 武器池(R)
        if (weaponR.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponR.transform.childCount; i++)
            {
                weaponPoolR.Add(weaponR.transform.GetChild(i).GetComponent<ItemOnWeapon>());
            }
        }
    }

    /// <summary>
    /// 裝備上武器(舊版，專門測試用)
    /// BossTeleport外掛按鍵引用中
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    ItemOnWeapon EquipWeapon(WeaponType type, int id)
    {
        WeaponSetActiveOpen(type, id);//開啟選擇的武器
        switch (type)
        {
            case WeaponType.LeftTorch:
                CurrentTorchL_torch = GetWeapon(type, id);
                return CurrentTorchL_torch;
            case WeaponType.LeftShield:
                CurrentWeaponL_weaponL = GetWeapon(type, id);
                return CurrentWeaponL_weaponL;
            case WeaponType.RightSword:
                CurrentWeaponR_weaponR = GetWeapon(type, id);
                playerController.AutoSwitchWeaponR(type);//之後改註冊事件
                return CurrentWeaponR_weaponR;
            case WeaponType.BothHandsSword:
                CurrentWeaponR_weaponR = GetWeapon(type, id);
                playerController.AutoSwitchWeaponR(type);//之後改註冊事件
                return CurrentWeaponR_weaponR;
            default:
                Debug.Log("沒有設定到武器");
                return null;
        }
    }

    /// <summary>
    /// 開啟選擇的武器，並關閉其他同類型的武器
    /// 武器預先綁好在武器物件下，這裡顯示目前持有武器，其餘關閉
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id">武器id</param>
    void WeaponSetActiveOpen(WeaponType type, int id)
    {
        ItemOnWeapon weapon = null;
        switch (type)
        {
            case WeaponType.LeftTorch:
                torchPoolL.ForEach(v => v.gameObject.SetActive(false));
                weapon = torchPoolL.FirstOrDefault(v => v.weaponID == id);
                weapon.gameObject.SetActive(true);
                break;
            case WeaponType.LeftShield:
                weaponPoolL.ForEach(v => v.gameObject.SetActive(false));
                weapon = weaponPoolL.FirstOrDefault(v => v.weaponID == id);
                weapon.gameObject.SetActive(true);
                break;
            case WeaponType.RightSword:
                weaponPoolR.ForEach(v => v.gameObject.SetActive(false));
                weapon = weaponPoolR.FirstOrDefault(v => v.weaponID == id);
                weapon.gameObject.SetActive(true);
                break;
            case WeaponType.BothHandsSword:
                weaponPoolR.ForEach(v => v.gameObject.SetActive(false));
                weapon = weaponPoolR.FirstOrDefault(v => v.weaponID == id);
                weapon.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 抓出該武器物件(不會切換)
    /// </summary>
    /// <param name="type">武器類型</param>
    /// <param name="id">武器id</param>
    /// <returns></returns>
    ItemOnWeapon GetWeapon(WeaponType type, int id)
    {
        switch (type)
        {
            case WeaponType.LeftTorch:
                return torchPoolL.FirstOrDefault(v => v.weaponID == id);
            case WeaponType.LeftShield:
                return weaponPoolL.FirstOrDefault(v => v.weaponID == id);
            case WeaponType.RightSword:
                return weaponPoolR.FirstOrDefault(v => v.weaponID == id);
            case WeaponType.BothHandsSword:
                return weaponPoolR.FirstOrDefault(v => v.weaponID == id);
            default:
                return null;
        }
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
    public List<ItemOnWeapon> GetRandomThreeWeaponR()
    {
        int UsedWeaponType3Index = -1;//用過的雙手劍weaponPoolR 內的index
        int UsedWeaponType2Index = -1;//用過的單手劍weaponPoolR 內的index

        int randomIndex = UnityEngine.Random.Range(0, weaponPoolR.Count);
        List<ItemOnWeapon> randomThreeWeaponRPool = new List<ItemOnWeapon>();

        //雙手劍 && 不等於當前裝備右手武器
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (weaponPoolR[randomIndex].weaponType == WeaponType.BothHandsSword)
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
            if (weaponPoolR[randomIndex].weaponType == WeaponType.RightSword)
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
    public List<ItemOnWeapon> GetRandomThreeWeaponL()
    {
        int UsedWeaponLIndexX = -1;
        int randomIndex = UnityEngine.Random.Range(0, weaponPoolL.Count);
        List<ItemOnWeapon> randomThreeWeaponLPool = new List<ItemOnWeapon>();

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
    public void ChooseAndUseWeapon(ItemOnWeapon aWeapon)
    {
        WeaponType type = (WeaponType)aWeapon.weaponType;
        int id = aWeapon.weaponID;

        WeaponSetActiveOpen(type, id);//開啟選擇的武器

        switch (type)
        {
            case WeaponType.LeftTorch:
                CurrentTorchL_torch = GetWeapon(type, id);
                break;
            case WeaponType.LeftShield:
                CurrentWeaponL_weaponL = GetWeapon(type, id);
                //設置當前武器進武器格
                UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>()
                    .SetCurrentWeaponImage(aWeapon);
                break;
            case WeaponType.RightSword:
                CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右單手武器
                //設置當前武器進武器格
                UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>()
                    .SetCurrentWeaponImage(aWeapon);
                playerController.AutoSwitchWeaponR(type);
                break;
            case WeaponType.BothHandsSword:
                CurrentWeaponR_weaponR = GetWeapon(type, id);//當前右雙手武器
                //設置當前武器進武器格
                UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>()
                    .SetCurrentWeaponImage(aWeapon);
                playerController.AutoSwitchWeaponR(type);
                break;
            default:
                Debug.LogError("武器不存在");
                break;
        }
    }
    /*---------------------------------*/

    /// <summary>
    /// 清除玩家當前裝備的所有武器(除了火把)
    /// </summary>
    public void SetAllCurrentWeaponsEmpty()
    {
        CurrentWeaponL_weaponL = null;
        CurrentWeaponR_weaponR = null;
    }

    /// <summary>
    /// 開始時設置預設武器(遊戲展示用)
    /// </summary>
    public void SetDefaultWeaponFirst()
    {
        ItemOnWeapon weapon = EquipWeapon(WeaponType.BothHandsSword, 30);//設置id30號武器
        UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(weapon);//設置當前武器進武器格
    }

}
