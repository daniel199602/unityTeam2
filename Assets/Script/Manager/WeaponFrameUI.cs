using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponFrameUI : MonoBehaviour
{
    public Image imageTorch;//�Z���ϥ�(����)
    public Image imageWeaponR;//�Z���ϥ�(�k��)
    public Image imageWeaponL;//�Z���ϥ�(����)

    /// <summary>
    /// ��WeaponManager�q�LUImanager�ϥΡA��e�Z�����ܮɡA�]�m�Z����ϥ�
    /// </summary>
    public void SetCurrentWeaponImage(GameObject weapon)
    {
        if(weapon.GetComponent<ItemOnWeapon>().weaponType==3)//����C
        {
            imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
            imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[-2];
        }
        else if(weapon.GetComponent<ItemOnWeapon>().weaponType == 2)//���C
        {
            imageWeaponR.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
        }
        else if (weapon.GetComponent<ItemOnWeapon>().weaponType == 1)//��
        {
            imageWeaponL.sprite = UIManager.Instance().dicIDWeaponImage[weapon.GetComponent<ItemOnWeapon>().weaponID];
        }
    }

}
