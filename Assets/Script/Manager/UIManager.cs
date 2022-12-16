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

    public RectTransform OneOfThree;

    public Image x;//三選一之0，武器圖

    public Dictionary<int,Sprite> dicIDWeaponImage;//ID 找 武器圖 字典
    public Transform weaponButton;//武器卡(按鈕)
    private TMP_Text[] weaponTextArray;//武器文字陣列

    private bool boolgggg= true;

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
        weaponTextArray = weaponButton.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
    }

    private void Start()
    {
        dicIDWeaponImage.Add(20, Resources.Load<Sprite>("iconTest_20"));
        dicIDWeaponImage.Add(30, Resources.Load<Sprite>("iconTest_30"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            x.sprite = dicIDWeaponImage[30];
            Time.timeScale = 0.001f;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            x.sprite = dicIDWeaponImage[20];

            weaponTextArray[0].text = "weapon Ya";
            weaponTextArray[1].text = "" + 10000;
            weaponTextArray[2].text = "" + 1;
            weaponTextArray[3].text = "" + 99;
            weaponTextArray[4].text = "" + 77;

            Time.timeScale = 0.001f;

            OneOfThree.gameObject.SetActive(boolgggg);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            OneOfThree.gameObject.SetActive(!boolgggg);
            Time.timeScale = 1f;
        }
    }
}
