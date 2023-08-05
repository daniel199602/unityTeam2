using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponFrameUI : MonoBehaviour
{
    public Image imageTorch;//武器圖示(火把)
    public Image imageWeaponR;//武器圖示(右手)
    public Image imageWeaponL;//武器圖示(左手)

    /// <summary>
    /// 讓WeaponManager通過UImanager使用，當前武器改變時，設置武器格圖示
    /// </summary>
    public void SetCurrentWeaponImage(ItemOnWeapon weapon)
    {
        switch(weapon.weaponType)
        {
            case WeaponType.LeftShield:
                imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[weapon.weaponID];
                break;
            case WeaponType.RightSword:
                imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.weaponID];
                break;
            case WeaponType.BothHandsSword:
                imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.weaponID];
                imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[-2];
                break;
        }
    }

    /// <summary>
    /// 將武器格的圖示全部清空
    /// </summary>
    public void SetEmptyWeaponImage()
    {
        imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[-1];//右手武器(劍)Frame
        imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[-1];//左手武器(盾)Frame
    }

}
