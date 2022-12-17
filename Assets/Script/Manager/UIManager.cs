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

    public Dictionary<int, Sprite> dicIDWeaponImage;//ID �� �Z���� �r��

    //public Transform weaponButtonX;//�Z���d(���s)
    //public Transform weaponButtonY;//�Z���d(���s)
    //public Transform weaponButtonZ;//�Z���d(���s)
    //public Image imageX;//�T��@��0�A��ܪZ���ϥ�
    //public Image imageY;//�T��@��1�A��ܪZ���ϥ�
    //public Image imageZ;//�T��@��2�A��ܪZ���ϥ�

    //private TMP_Text[] weaponDataArrayX;//�Z����r�}�C
    //private TMP_Text[] weaponDataArrayY;//�Z����r�}�C
    //private TMP_Text[] weaponDataArrayZ;//�Z����r�}�C

    //[HideInInspector] public GameObject weaponX;//�Ȧs���s��m�Z��
    //[HideInInspector] public GameObject weaponY;//�Ȧs���s��m�Z��
    //[HideInInspector] public GameObject weaponZ;//�Ȧs���s��m�Z��

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
        //weaponDataArrayX = weaponButtonX.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��
        //weaponDataArrayY = weaponButtonY.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��
        //weaponDataArrayZ = weaponButtonZ.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��

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
        //���եζ}����E
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
    /// �}�ҤT��@����
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        OneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//�]�m�k��Z���T��@

        OneOfThreePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// �����T��@����
    /// </summary>
    public void OneOfThreeUIClose()
    {
        OneOfThreePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// �]�m�k��Z���T��@���
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
//    /// �]�m����Z���T��@���
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
//    /// �I���Z�����sX�A�]�m�Z��
//    /// </summary>
//    public void ClickWeaponBtnX()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponX);
//        OneOfThreeUIClose();
//    }
//    /// <summary>
//    /// �I���Z�����sY�ƥ�A�]�m�Z��
//    /// </summary>
//    public void ClickWeaponBtnY()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponY);
//        OneOfThreeUIClose();
//    }
//    /// <summary>
//    /// �I���Z�����sZ�ƥ�A�]�m�Z��
//    /// </summary>
//    public void ClickWeaponBtnZ()
//    {
//        WeaponManager.Instance().ChooseAndUseWeapon(weaponZ);
//        OneOfThreeUIClose();
//    }


}
