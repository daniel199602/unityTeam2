using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponOneOfThreeUI : MonoBehaviour
{
    public Transform weaponButtonX;//武器卡(按鈕)
    public Transform weaponButtonY;//武器卡(按鈕)
    public Transform weaponButtonZ;//武器卡(按鈕)
    public Image imageX;//三選一之0，顯示武器圖示
    public Image imageY;//三選一之1，顯示武器圖示
    public Image imageZ;//三選一之2，顯示武器圖示

   
    private TMP_Text[] weaponDataArrayX;//武器文字陣列
    private TMP_Text[] weaponDataArrayY;//武器文字陣列
    private TMP_Text[] weaponDataArrayZ;//武器文字陣列

    [HideInInspector] public GameObject weaponX;//暫存按鈕位置武器
    [HideInInspector] public GameObject weaponY;//暫存按鈕位置武器
    [HideInInspector] public GameObject weaponZ;//暫存按鈕位置武器

    private void Awake()
    {
        weaponDataArrayX = weaponButtonX.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
        weaponDataArrayY = weaponButtonY.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找
        weaponDataArrayZ = weaponButtonZ.GetComponentsInChildren<TMP_Text>();//依照子對象順序查找

        transform.gameObject.SetActive(false);
    }


    /// <summary>
    /// 設置右手武器三選一資料
    /// </summary>
    public void SetRandomThreeWeaponR()
    {
        int i = 0;
        foreach (GameObject weaponR in WeaponManager.Instance().GetRandomThreeWeaponR())
        {
            if (i == 0)
            {
                weaponX = weaponR;
                imageX.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayX[0].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayX[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayX[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayX[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayX[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;
            }
            if (i == 1)
            {
                weaponY = weaponR;
                imageY.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayY[0].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayY[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayY[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayY[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayY[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;

            }
            if (i == 2)
            {
                weaponZ = weaponR;
                imageZ.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayZ[0].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayZ[1].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayZ[2].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayZ[3].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayZ[4].text = "" + weaponR.GetComponent<ItemOnWeapon>().weaponRadius;
            }
            i++;
        }
    }

    /// <summary>
    /// 設置左手武器三選一資料
    /// </summary>
    public void SetRandomThreeWeaponL()
    {
        int i = 0;
        foreach (GameObject weaponL in WeaponManager.Instance().GetRandomThreeWeaponL())
        {
            if (i == 0)
            {
                weaponX = weaponL;
                imageX.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayX[0].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayX[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayX[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayX[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayX[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;
            }
            if (i == 1)
            {
                weaponY = weaponL;
                imageY.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayY[0].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayY[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayY[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayY[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayY[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;

            }
            if (i == 2)
            {
                weaponZ = weaponL;
                imageZ.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.GetComponent<ItemOnWeapon>().weaponID];
                weaponDataArrayZ[0].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponName;
                weaponDataArrayZ[1].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                weaponDataArrayZ[2].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                weaponDataArrayZ[3].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
                weaponDataArrayZ[4].text = "" + weaponL.GetComponent<ItemOnWeapon>().weaponRadius;
            }
            i++;
        }
    }

    /// <summary>
    /// 點擊武器按鈕X，設置武器
    /// </summary>
    public void ClickWeaponBtnX()
    {
        WeaponManager.Instance().ChooseAndUseWeapon(weaponX);
        if(weaponX.GetComponent<ItemOnWeapon>().weaponType==2)
        {
            SetRandomThreeWeaponL();
        }
        else
        {
            UIManager.Instance().OneOfThreeUIClose();
        }
    }
    /// <summary>
    /// 點擊武器按鈕Y事件，設置武器
    /// </summary>
    public void ClickWeaponBtnY()
    {
        WeaponManager.Instance().ChooseAndUseWeapon(weaponY);
        if (weaponY.GetComponent<ItemOnWeapon>().weaponType == 2)
        {
            SetRandomThreeWeaponL();
        }
        else
        {
            UIManager.Instance().OneOfThreeUIClose();
        }
    }
    /// <summary>
    /// 點擊武器按鈕Z事件，設置武器
    /// </summary>
    public void ClickWeaponBtnZ()
    {
        WeaponManager.Instance().ChooseAndUseWeapon(weaponZ);
        if (weaponZ.GetComponent<ItemOnWeapon>().weaponType == 2)
        {
            SetRandomThreeWeaponL();
        }
        else
        {
            UIManager.Instance().OneOfThreeUIClose();
        }
    }
}
