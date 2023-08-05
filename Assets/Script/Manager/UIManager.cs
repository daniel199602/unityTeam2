using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    public static UIManager Instance() { return mInstance; }

    public RectTransform fadeColor;//暗幕
    public RectTransform weaponOneOfThreePanel;//三選一面板
    public RectTransform weaponFramePanel;//武器格面板
    public RectTransform quitGamePanel;//離開遊戲面板
    public RectTransform gameMenuPanel;//遊戲菜單面板

    public Dictionary<int, Sprite> dicIDWeaponImage;//ID 找 武器圖 字典

    bool isOpenQuitGameUI = false;//是否打開離開選單，預設false

    private void Awake()
    {
        if (mInstance != null)
        {
            //Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            Debug.LogWarning("有兩個相同的singleton物件,UIManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(gameObject);


        dicIDWeaponImage = new Dictionary<int, Sprite>();
    }

    private void Start()
    {
        dicIDWeaponImage.Add(10, Resources.Load<Sprite>("w_10"));
        dicIDWeaponImage.Add(11, Resources.Load<Sprite>("w_11"));
        dicIDWeaponImage.Add(12, Resources.Load<Sprite>("w_12"));
        dicIDWeaponImage.Add(13, Resources.Load<Sprite>("w_13"));
        dicIDWeaponImage.Add(20, Resources.Load<Sprite>("w_20"));
        dicIDWeaponImage.Add(21, Resources.Load<Sprite>("w_21"));
        dicIDWeaponImage.Add(22, Resources.Load<Sprite>("w_22"));
        dicIDWeaponImage.Add(23, Resources.Load<Sprite>("w_23"));
        dicIDWeaponImage.Add(30, Resources.Load<Sprite>("w_30"));
        dicIDWeaponImage.Add(31, Resources.Load<Sprite>("w_31"));
        dicIDWeaponImage.Add(32, Resources.Load<Sprite>("w_32"));
        dicIDWeaponImage.Add(33, Resources.Load<Sprite>("w_33"));
        dicIDWeaponImage.Add(-1, Resources.Load<Sprite>("w_none"));
        dicIDWeaponImage.Add(-2, Resources.Load<Sprite>("w_close"));
    }

    private void Update()
    {

        //按下esc鍵
        if (Input.GetKeyDown("escape"))
        {
            isOpenQuitGameUI = !isOpenQuitGameUI;
            if (isOpenQuitGameUI)
            {
                QuitGameUIOpen();
            }
            else if(!isOpenQuitGameUI)
            {
                QuitGameUIClose();
            }
        }

    }

    /// <summary>
    /// 給寶箱使用，設置武器三選一圖示
    /// </summary>
    public void OpenOneOfThreeAndChooseWeapon()
    {
        OneOfThreeUIOpen();
        weaponOneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//設置右手武器三選一
    }

    /// <summary>
    /// 開啟遊戲菜單介面
    /// </summary>
    public void GameMenuPanelOpen()
    {
        gameMenuPanel.gameObject.SetActive(true);
    }
    /// <summary>
    /// 關閉遊戲菜單介面
    /// </summary>
    public void GameMenuPanelClose()
    {
        gameMenuPanel.gameObject.SetActive(false);
    }


    /// <summary>
    /// 開啟三選一介面，時間調慢
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        weaponOneOfThreePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// 關閉三選一介面，時間恢復正常
    /// </summary>
    public void OneOfThreeUIClose()
    {
        weaponOneOfThreePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 開啟離開遊戲介面，時間調慢
    /// </summary>
    public void QuitGameUIOpen()
    {
        quitGamePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// 關閉離開遊戲介面，時間恢復正常
    /// </summary>
    public void QuitGameUIClose()
    {
        quitGamePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isOpenQuitGameUI = false;//是否打開離開選單，切成false
    }

}
