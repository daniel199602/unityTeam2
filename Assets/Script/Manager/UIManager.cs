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

    public RectTransform OneOfThreePanel;//�T��@����

    public Transform weaponButtonX;//�Z���d(���s)
    public Transform weaponButtonY;//�Z���d(���s)
    public Transform weaponButtonZ;//�Z���d(���s)
    public Image imageX;//�T��@��0�A��ܪZ���ϥ�
    public Image imageY;//�T��@��1�A��ܪZ���ϥ�
    public Image imageZ;//�T��@��2�A��ܪZ���ϥ�

    public Dictionary<int,Sprite> dicIDWeaponImage;//ID �� �Z���� �r��
    private TMP_Text[] weaponTextArray;//�Z����r�}�C

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
        weaponTextArray = weaponButtonX.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��
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
    /// �}�ҤT��@����
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        OneOfThreePanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// �����T��@����
    /// </summary>
    public void OneOfThreeUIClose()
    {
        OneOfThreePanel.gameObject.SetActive(false);
    }

}
