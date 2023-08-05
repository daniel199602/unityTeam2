﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class WeaponOneOfThreeUI : MonoBehaviour
{
    enum ButtonType
    {
        X,
        Y,
        Z
    }

    [SerializeField] Button weaponButtonX = null;//武器卡(按鈕)
    [SerializeField] Button weaponButtonY = null;//武器卡(按鈕)
    [SerializeField] Button weaponButtonZ = null;//武器卡(按鈕)
    [SerializeField] Image imageX = null;//三選一之0，顯示武器圖示
    [SerializeField] Image imageY = null;//三選一之1，顯示武器圖示
    [SerializeField] Image imageZ = null;//三選一之2，顯示武器圖示

    TMP_Text[] weaponDataArrayX;//武器文字陣列
    TMP_Text[] weaponDataArrayY;//武器文字陣列
    TMP_Text[] weaponDataArrayZ;//武器文字陣列

    ItemOnWeapon weaponX; //該按鈕的武器
    ItemOnWeapon weaponY; //該按鈕的武器
    ItemOnWeapon weaponZ; //該按鈕的武器

    private void Start()
    {
        weaponButtonX.onClick.AddListener(() => OnButtonEvent(ButtonType.X));
        weaponButtonY.onClick.AddListener(() => OnButtonEvent(ButtonType.Y));
        weaponButtonZ.onClick.AddListener(() => OnButtonEvent(ButtonType.Z));

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
        foreach (ItemOnWeapon weaponR in WeaponManager.Instance.GetRandomThreeWeaponR())
        {
            if (i == 0)
            {
                weaponX = weaponR;
                imageX.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.weaponID];
                weaponDataArrayX[0].text = "" + weaponR.weaponName;
                weaponDataArrayX[1].text = "" + weaponR.weaponDamage_instant;
                weaponDataArrayX[2].text = "" + weaponR.weaponDamage_delay;
                weaponDataArrayX[3].text = "" + weaponR.weaponAngle;
                weaponDataArrayX[4].text = "" + weaponR.weaponRadius;
            }
            if (i == 1)
            {
                weaponY = weaponR;
                imageY.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.weaponID];
                weaponDataArrayY[0].text = "" + weaponR.weaponName;
                weaponDataArrayY[1].text = "" + weaponR.weaponDamage_instant;
                weaponDataArrayY[2].text = "" + weaponR.weaponDamage_delay;
                weaponDataArrayY[3].text = "" + weaponR.weaponAngle;
                weaponDataArrayY[4].text = "" + weaponR.weaponRadius;

            }
            if (i == 2)
            {
                weaponZ = weaponR;
                imageZ.sprite = UIManager.Instance().dicIDWeaponImage[weaponR.weaponID];
                weaponDataArrayZ[0].text = "" + weaponR.weaponName;
                weaponDataArrayZ[1].text = "" + weaponR.weaponDamage_instant;
                weaponDataArrayZ[2].text = "" + weaponR.weaponDamage_delay;
                weaponDataArrayZ[3].text = "" + weaponR.weaponAngle;
                weaponDataArrayZ[4].text = "" + weaponR.weaponRadius;
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
        foreach (ItemOnWeapon weaponL in WeaponManager.Instance.GetRandomThreeWeaponL())
        {
            if (i == 0)
            {
                weaponX = weaponL;
                imageX.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.weaponID];
                weaponDataArrayX[0].text = "" + weaponL.weaponName;
                weaponDataArrayX[1].text = "" + weaponL.weaponDamage_instant;
                weaponDataArrayX[2].text = "" + weaponL.weaponDamage_delay;
                weaponDataArrayX[3].text = "" + weaponL.weaponAngle;
                weaponDataArrayX[4].text = "" + weaponL.weaponRadius;
            }
            if (i == 1)
            {
                weaponY = weaponL;
                imageY.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.weaponID];
                weaponDataArrayY[0].text = "" + weaponL.weaponName;
                weaponDataArrayY[1].text = "" + weaponL.weaponDamage_instant;
                weaponDataArrayY[2].text = "" + weaponL.weaponDamage_delay;
                weaponDataArrayY[3].text = "" + weaponL.weaponAngle;
                weaponDataArrayY[4].text = "" + weaponL.weaponRadius;

            }
            if (i == 2)
            {
                weaponZ = weaponL;
                imageZ.sprite = UIManager.Instance().dicIDWeaponImage[weaponL.weaponID];
                weaponDataArrayZ[0].text = "" + weaponL.weaponName;
                weaponDataArrayZ[1].text = "" + weaponL.weaponDamage_instant;
                weaponDataArrayZ[2].text = "" + weaponL.weaponDamage_delay;
                weaponDataArrayZ[3].text = "" + weaponL.weaponAngle;
                weaponDataArrayZ[4].text = "" + weaponL.weaponRadius;
            }
            i++;
        }
    }

    /// <summary>
    /// 按鈕事件
    /// </summary>
    void OnButtonEvent(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.X:
                WeaponManager.Instance.ChooseAndUseWeapon(weaponX);
                if (weaponX.weaponType == WeaponType.RightSword)
                {
                    SetRandomThreeWeaponL();
                }
                else
                {
                    UIManager.Instance().OneOfThreeUIClose();
                }
                break;
            case ButtonType.Y:
                WeaponManager.Instance.ChooseAndUseWeapon(weaponY);
                if (weaponY.weaponType == WeaponType.RightSword)
                {
                    SetRandomThreeWeaponL();
                }
                else
                {
                    UIManager.Instance().OneOfThreeUIClose();
                }
                break;
            case ButtonType.Z:
                WeaponManager.Instance.ChooseAndUseWeapon(weaponZ);
                if (weaponZ.weaponType == WeaponType.RightSword)
                {
                    SetRandomThreeWeaponL();
                }
                else
                {
                    UIManager.Instance().OneOfThreeUIClose();
                }
                break;
        }
    }
}
