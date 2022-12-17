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

    public Transform weaponButtonX;//武器卡(按鈕)
    public Transform weaponButtonY;//武器卡(按鈕)
    public Transform weaponButtonZ;//武器卡(按鈕)
    public Image imageX;//三選一之0，顯示武器圖示
    public Image imageY;//三選一之1，顯示武器圖示
    public Image imageZ;//三選一之2，顯示武器圖示

    public Dictionary<int,Sprite> dicIDWeaponImage;//ID 找 武器圖 字典
    private TMP_Text[] weaponTextArray;//武器文字陣列

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
        weaponTextArray = weaponButtonX.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
    }

    private void Start()
    {
        dicIDWeaponImage.Add(20, Resources.Load<Sprite>("w_20"));
        dicIDWeaponImage.Add(30, Resources.Load<Sprite>("iconTest_30"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            imageX.sprite = dicIDWeaponImage[30];
            Time.timeScale = 0.001f;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            imageX.sprite = dicIDWeaponImage[20];

            weaponTextArray[0].text = "weapon Ya";
            weaponTextArray[1].text = "" + 10000;
            weaponTextArray[2].text = "" + 1;
            weaponTextArray[3].text = "" + 99;
            weaponTextArray[4].text = "" + 77;

            Time.timeScale = 0.001f;

            OneOfThreePanel.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OneOfThreePanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }

    /// <summary>
    /// 開啟三選一介面
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        OneOfThreePanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 關閉三選一介面
    /// </summary>
    public void OneOfThreeUIClose()
    {
        OneOfThreePanel.gameObject.SetActive(false);
    }

}
