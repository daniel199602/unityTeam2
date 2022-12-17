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

    public RectTransform OneOfThreePanel;//三選一面版

    public Dictionary<int, Sprite> dicIDWeaponImage;//ID 找 武器圖 字典

    //public Transform weaponButtonX;//武器卡(按鈕)
    //public Transform weaponButtonY;//武器卡(按鈕)
    //public Transform weaponButtonZ;//武器卡(按鈕)
    //public Image imageX;//三選一之0，顯示武器圖示
    //public Image imageY;//三選一之1，顯示武器圖示
    //public Image imageZ;//三選一之2，顯示武器圖示

    //private TMP_Text[] weaponDataArrayX;//武器文字陣列
    //private TMP_Text[] weaponDataArrayY;//武器文字陣列
    //private TMP_Text[] weaponDataArrayZ;//武器文字陣列

    //[HideInInspector] public GameObject weaponX;//暫存按鈕位置武器
    //[HideInInspector] public GameObject weaponY;//暫存按鈕位置武器
    //[HideInInspector] public GameObject weaponZ;//暫存按鈕位置武器

    bool isOpenFrame = false;

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
        //weaponDataArrayX = weaponButtonX.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
        //weaponDataArrayY = weaponButtonY.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
        //weaponDataArrayZ = weaponButtonZ.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找

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
            isOpenFrame = !isOpenFrame;

            if (isOpenFrame)
            {
                OneOfThreeUIOpen();
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
        OneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//設置右手武器三選一

        OneOfThreePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// 關閉三選一介面
    /// </summary>
    public void OneOfThreeUIClose()
    {
        OneOfThreePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// 設置右手武器三選一資料
    /// </summary>
//    public void SetRandomThreeWeaponR()
//    {
//        int i = 0;
//        foreach (GameObject weaponR in WeaponManager.Instance().GetRandomThreeWeaponR())
//        {
//            if (i == 0)
//            {
//                weaponX = weaponR;
//                imageX.sprite = dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayX[0].text = weaponR.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayX[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayX[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayX[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayX[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;
//            }
//            if (i == 1)
//            {
//                weaponY = weaponR;
//                imageY.sprite = dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayY[0].text = weaponR.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayY[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayY[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayY[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayY[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;

//            }
//            if (i == 2)
//            {
//                weaponZ = weaponR;
//                imageZ.sprite = dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayZ[0].text = weaponR.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayZ[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayZ[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayZ[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayZ[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;
//            }
//            i++;
//        }
//    //}

//    /// <summary>
//    /// 設置左手武器三選一資料
//    /// </summary>
//    public void SetRandomThreeWeaponL()
//    {
//        int i = 0;
//        foreach (GameObject weaponL in WeaponManager.Instance().GetRandomThreeWeaponL())
//        {
//            if (i == 0)
//            {
//                imageX.sprite = dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayX[0].text = weaponL.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayX[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayX[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayX[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayX[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;
//            }
//            if (i == 1)
//            {
//                imageY.sprite = dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayY[0].text = weaponL.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayY[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayY[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayY[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayY[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;

//            }
//            if (i == 2)
//            {
//                imageZ.sprite = dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
//                weaponDataArrayZ[0].text = weaponL.GetComponent<ItemOnWeapon>().weaponName;
//                weaponDataArrayZ[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
//                weaponDataArrayZ[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
//                weaponDataArrayZ[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
//                weaponDataArrayZ[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;
//            }
//            i++;
//        }
//    }

//    /// <summary>
//    /// 點擊武器按鈕X，設置武器
//    /// </summary>
//    public void ClickWeaponBtnX()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponX);
//        OneOfThreeUIClose();
//    }
//    /// <summary>
//    /// 點擊武器按鈕Y事件，設置武器
//    /// </summary>
//    public void ClickWeaponBtnY()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponY);
//        OneOfThreeUIClose();
//    }
//    /// <summary>
//    /// 點擊武器按鈕Z事件，設置武器
//    /// </summary>
//    public void ClickWeaponBtnZ()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponZ);
//        OneOfThreeUIClose();
//    }


}
