using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    public GameObject torchL;//�j���⪺����
    public GameObject weaponL;//����
    public GameObject weaponR;//�k��

    public List<GameObject> torchPoolL;//�����
    public List<GameObject> weaponPoolL;//����Z����
    public List<GameObject> weaponPoolR;//�k��Z����

    /*�D�`�D�`���n���]�w!!_�|�v�TPlayerController���ʵeLayer������*/
    //�������_type 0, id �d�� 0~9 ���
    //������_type 1, id �d�� 10~19 ���
    //�k���C_type 2, id �d�� 20~29 ���
    //�k����C_type 3, id �d�� 30~39 ���
    /**/

    /// <summary>
    /// ��e�ϥΤ�������
    /// </summary>
    public GameObject CurrentTorchL_torch { private set; get; }
    /// <summary>
    /// ��e�ϥΤ�������Z��
    /// </summary>
    public GameObject CurrentWeaponL_weaponL { private set; get; }
    /// <summary>
    /// ��e�ϥΤ����k��Z��
    /// PlayerController�P�_���a��eAnimator Layer���̾ڡA�k��Z��type==2 or type==3
    /// </summary>
    public GameObject CurrentWeaponR_weaponR { private set; get; }

    GameObject player;//�s���a

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);

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

        player = GameManager.Instance().PlayerStart;//��쪱�a
    }

    void Start()
    {
        //���a��l�Z���]�w
        ChooseAndUseWeapon(0, 0);//��l����
        ChooseAndUseWeapon(1, 10);//��l�޵P
        ChooseAndUseWeapon(2, 20);//��l�k����C
    }


    /*1213���եΡA����R-------------------------------*/
    [HeaderAttribute("Test_WeaponSwitch")]
    public int test_type1_id = 10;
    [Range(2, 3)] public int test_type23 = 2;
    public int test_type2_id = 20;
    public int test_type3_id = 30;
    /// <summary>
    /// ���եΨ禡�A�M���Ω�Z�������Ϊ�����
    /// </summary>
    private void Test_ChooseAndUseWeapon(int type1_id, int type2or3, int type2_id,int type3_id)
    {
        
        ChooseAndUseWeapon(1, type1_id);//�k��_�޵P
        if (type2or3 == 2)
        {
            ChooseAndUseWeapon(2, type2_id);//����_���C
        }
        else if(type2or3 == 3)
        {
            ChooseAndUseWeapon(3, type3_id);//����_����C
        }

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Test_ChooseAndUseWeapon(test_type1_id, test_type23, test_type2_id, test_type3_id);
        }
    }
    /*-------------------------------*/


    /// <summary>
    /// ��ܨèϥθӪZ��
    /// (�T��@����|�Ψ�o�Ө禡)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void ChooseAndUseWeapon(int type, int id)
    {
        WeaponSetActiveOpen(type, id);//�}�ҿ�ܪ��Z��

        if (type == 0)
            this.CurrentTorchL_torch = GetWeapon(type, id);//��e����
        if (type == 1)
            this.CurrentWeaponL_weaponL = GetWeapon(type, id);//��e����Z��
        if (type == 2)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//��e�k���Z��
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
        if (type == 3)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//��e�k����Z��
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
    }

    /// <summary>
    /// �}�ҿ�ܪ��Z���A��������L�P�������Z��
    /// </summary>
    /// <param name="type">�Z������</param>
    /// <param name="id">�Z��id</param>
    public void WeaponSetActiveOpen(int type, int id)
    {
        if (type == 0)//�������_type0
        {
            foreach (GameObject weapon in torchPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("���⪺type:" + type+"id:"+id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 1)//������_type1
        {
            foreach (GameObject weapon in weaponPoolL)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("������type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 2)//�k���C_type2
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("�k���C type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
        if (type == 3)//�k����C_type3
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    weapon.SetActive(true);
                    Debug.LogWarning("�k����C type:" + type + "id:" + id);//debug
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
    }


    /// <summary>
    /// ��X�ӪZ������(���|����)
    /// </summary>
    /// <param name="type">�Z������</param>
    /// <param name="id">�Z��id</param>
    /// <returns></returns>
    public GameObject GetWeapon(int type, int id)
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
        if (type == 2)//�k���C_2
        {
            foreach (GameObject weapon in weaponPoolR)
            {
                if (weapon.GetComponent<ItemOnWeapon>().weaponID == id)
                {
                    return weapon;
                }
            }
        }
        if(type == 3)//�k����C_3
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


}
