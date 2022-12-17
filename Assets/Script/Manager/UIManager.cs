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

    public RectTransform weaponOneOfThreePanel;//三選一面板
    public RectTransform weaponFramePanel;//武器格面板

    public Dictionary<int, Sprite> dicIDWeaponImage;//ID 找 武器圖 字典

    bool isOpenOneOfThreeFrame = false;

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
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
        //測試用開關按E
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpenOneOfThreeFrame = !isOpenOneOfThreeFrame;

            if (isOpenOneOfThreeFrame)
            {
                OneOfThreeUIOpen();
                weaponOneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//設置右手武器三選一
            }
            else
            {
                OneOfThreeUIClose();
            }
        }
        
    }

    /// <summary>
    /// 開啟三選一介面
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        weaponOneOfThreePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// 關閉三選一介面
    /// </summary>
    public void OneOfThreeUIClose()
    {
        weaponOneOfThreePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

}
