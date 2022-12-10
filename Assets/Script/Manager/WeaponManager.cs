using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    /*��X�Ҧ��Z�����ƭȦs�b�o*/
    public GameObject torchL;//�j��������
    public GameObject weaponL;//����
    public GameObject weaponR;//�k��

    public List<GameObject> torchPoolL;
    public List<GameObject> weaponPoolL;
    public List<GameObject> weaponPoolR;

    /*�]�@�ӱ��f�A����Ҧ��Z�����}������(���F����)�CSetActive*/
    public void WeaponSetActiveOpen(int id)
    {
        foreach (GameObject weapon in weaponPoolL)
        {
            if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
            {
                weapon.SetActive(true);
                //int damage_instant = weapon.GetComponent<ItemOnWeapon>().weaponDamage_instant;
                //int damage_delay = weapon.GetComponent<ItemOnWeapon>().weaponDamage_delay;
                //float angle = weapon.GetComponent<ItemOnWeapon>().weaponAngle;
                //float radius = weapon.GetComponent<ItemOnWeapon>().weaponRadius;
            }
        }
    }

    /*�]�@�ӱ��f�A��X�ӪZ���A�ǵ�CharacterAttackManager�ζǵ�GameManager*/
    /// <summary>
    /// ����Z���ƭ�
    /// </summary>
    /// <param name="type">�Z������</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public GameObject GetWeaponNum(int type,int id)
    {
        if (type == 0)//��������_0
        {
            foreach (GameObject weapon in torchPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if (type == 1)//������_1
        {
            foreach (GameObject weapon in weaponPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if (type == 2||type == 3)//�k���C_2 or �k����C_3
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        
        return null;
    }

    private void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        //�������s�i �����(L)
        if (torchL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < torchL.transform.childCount; i++)
            {
                torchPoolL.Add(torchL.transform.GetChild(i).gameObject);
            }
        }
        //����Z���s�i �Z����(L)
        if (weaponL.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponL.transform.childCount; i++)
            {
                weaponPoolL.Add(weaponL.transform.GetChild(i).gameObject);
            }
        }
        //�k��Z���s�i �Z����(R)
        if (weaponR.transform.GetChild(0).gameObject)
        {
            for (int i = 0; i < weaponR.transform.childCount; i++)
            {
                weaponPoolR.Add(weaponR.transform.GetChild(i).gameObject);
            }
        }
    }

    void Update()
    {
        
    }
}
