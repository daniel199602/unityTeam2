using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WeaponOneOfThreeUI : MonoBehaviour
{
    public Transform weaponButtonX;//�Z���d(���s)
    public Transform weaponButtonY;//�Z���d(���s)
    public Transform weaponButtonZ;//�Z���d(���s)
    public Image imageX;//�T��@��0�A��ܪZ���ϥ�
    public Image imageY;//�T��@��1�A��ܪZ���ϥ�
    public Image imageZ;//�T��@��2�A��ܪZ���ϥ�

   
    private TMP_Text[] weaponDataArrayX;//�Z����r�}�C
    private TMP_Text[] weaponDataArrayY;//�Z����r�}�C
    private TMP_Text[] weaponDataArrayZ;//�Z����r�}�C

    [HideInInspector] public GameObject weaponX;//�Ȧs���s��m�Z��
    [HideInInspector] public GameObject weaponY;//�Ȧs���s��m�Z��
    [HideInInspector] public GameObject weaponZ;//�Ȧs���s��m�Z��

    private void Awake()
    {
        weaponDataArrayX = weaponButtonX.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��
        weaponDataArrayY = weaponButtonY.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��
        weaponDataArrayZ = weaponButtonZ.GetComponentsInChildren<TMP_Text>();//�̷Ӥl��H���Ǭd��

        transform.gameObject.SetActive(false);
    }


    /// <summary>
    /// �]�m�k��Z���T��@���
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
    /// �]�m����Z���T��@���
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
    /// �I���Z�����sX�A�]�m�Z��
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
    /// �I���Z�����sY�ƥ�A�]�m�Z��
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
    /// �I���Z�����sZ�ƥ�A�]�m�Z��
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
