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
    public void SetCurrentWeaponImage(GameObject weapon)
    {
        if(weapon.GetComponent<ItemOnWeapon>().weaponType==3)//雙手劍
        {
            imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
            imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[-2];
        }
        else if(weapon.GetComponent<ItemOnWeapon>().weaponType == 2)//單手劍
        {
            imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
        }
        else if (weapon.GetComponent<ItemOnWeapon>().weaponType == 1)//盾
        {
            imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
        }
    }

}
