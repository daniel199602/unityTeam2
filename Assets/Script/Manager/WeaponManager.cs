using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private static WeaponManager mInstance;
    public static WeaponManager Instance() { return mInstance; }

    private GameObject player;//�s���a

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


    /*1213���եΡA����R-------------------------------*/
    [HeaderAttribute("Test_WeaponSwitch( Press keyCode.L )")]
    public int test_type1_id = 10;
    [Range(2, 3)] public int test_type23 = 2;
    public int test_type2_id = 20;
    public int test_type3_id = 30;

    /// <summary>
    /// ���եΨ禡�A�M���Ω�Z�������Ϊ�����
    /// </summary>
    private void Test_ChooseAndUseWeapon(int type1_id, int type2or3, int type2_id, int type3_id)
    {
        ChooseAndUseWeaponTest(1, type1_id);//�k��_�޵P
        if (type2or3 == 2)
        {
            ChooseAndUseWeaponTest(2, type2_id);//����_���C
        }
        else if (type2or3 == 3)
        {
            ChooseAndUseWeaponTest(3, type3_id);//����_����C
        }
    }
    /*-----------------------------------------------------*/

    private void Awake()
    {
        if (mInstance != null)
        {
            //Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            Debug.LogWarning("����ӬۦP��singleton����,WeaponManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);

        AddAllWeaponsInTheirWeaponPool();//�N�Ҧ��Z���A���O�[�J�L�̦U�۪��Z����
    }

    void Start()
    {
        player = GameManager.Instance().PlayerStart;//��쪱�a

        //���a��l�Z���]�w
        ChooseAndUseWeaponTest(0, 0);//��l����

        //�g���T��@��A�o��N�ѤT��@�ӳ]�w�A����R
        //ChooseAndUseWeaponTest(1, 10);//��l�޵P
        //ChooseAndUseWeaponTest(2, 20);//��l�k����C
        //ChooseAndUseWeaponTest(3, 31);//��l�k������C
    }

    private void Update()
    {
        ////��L����������inspector��e��type,id�Z��
        //if(Input.GetKeyDown(KeyCode.L))
        //{
        //    Test_ChooseAndUseWeapon(test_type1_id, test_type23, test_type2_id, test_type3_id);
        //}
    }

    /// <summary>
    /// �N�Ҧ��Z���A���O�[�J�L�̦U�۪��Z����
    /// </summary>
    private void AddAllWeaponsInTheirWeaponPool()
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




    /// <summary>
    /// ��ܨèϥθӪZ��(���ϥΡA�M�����ե�)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    public void ChooseAndUseWeaponTest(int type, int id)
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

    /*�Z���T��@*/
    //���涶��: �n�b�Z�����짹�Ҧ��Z������

    //�Ĥ@�B����:
    //�H���q weaponPoolR ��3�䤣���ƪ��k��Z��(�C)
    //����1������C�A�A��1����C�A�A�H����1��k��C�A�B�p�G����e�˳ƪ��k��C�h�n�ư�
    /// <summary>
    /// �qweaponPoolR���A���H��������1������C�B1����C �B�A1��k��C
    /// �æ^�Ǥ@�� List<GameObject>
    /// </summary>
    public List<GameObject> GetRandomThreeWeaponR()
    {
        int UsedWeaponType3Index = -1;//�ιL������CweaponPoolR ����index
        int UsedWeaponType2Index = -1;//�ιL�����CweaponPoolR ����index

        int randomIndex = UnityEngine.Random.Range(0, weaponPoolR.Count);
        List<GameObject> randomThreeWeaponRPool = new List<GameObject>();

        //����C && �������e�˳ƥk��Z��
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (weaponPoolR[randomIndex].GetComponent<ItemOnWeapon>().weaponType == 3)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    UsedWeaponType3Index = randomIndex;
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        //���C && �������e�˳ƥk��Z��
        int randomIndexForSecondChoose = UnityEngine.Random.Range(0, weaponPoolR.Count);//�W�[�H����
        randomIndex = randomIndexForSecondChoose;
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (weaponPoolR[randomIndex].GetComponent<ItemOnWeapon>().weaponType == 2)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    UsedWeaponType2Index = randomIndex;
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        //�A�H����1��k��C && �������e�˳ƥk��Z�� && �����ƫe���Z��
        int randomIndexForThridChoose = UnityEngine.Random.Range(0, weaponPoolR.Count);//�W�[�H����
        randomIndex = randomIndexForThridChoose;
        for (int i = 0; i < weaponPoolR.Count; i++)
        {
            if (randomIndex != UsedWeaponType3Index && randomIndex != UsedWeaponType2Index)
            {
                if (weaponPoolR[randomIndex] != CurrentWeaponR_weaponR)
                {
                    randomThreeWeaponRPool.Add(weaponPoolR[randomIndex]);
                    break;
                }
            }
            if (randomIndex == (weaponPoolR.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolR.Count) randomIndex++;
        }
        return randomThreeWeaponRPool;
    }

    //�ĤG�B����:
    //�H���q weaponPoolR ��3�䤣���ƪ�����Z��(��)
    //�p�G����e�˳ƪ�����ޫh�n�ư�

    /// <summary>
    /// �qweaponPoolR���A���H��������3���
    /// �æ^�Ǥ@�� List<GameObject>
    /// </summary>
    public List<GameObject> GetRandomThreeWeaponL()
    {
        int UsedWeaponLIndexX = -1;
        int randomIndex = UnityEngine.Random.Range(0, weaponPoolL.Count);
        List<GameObject> randomThreeWeaponLPool = new List<GameObject>();

        //�޵PX
        for (int i = 0; i < weaponPoolL.Count; i++)
        {
            if (weaponPoolL[randomIndex] != CurrentWeaponL_weaponL)
            {
                randomThreeWeaponLPool.Add(weaponPoolL[randomIndex]);
                UsedWeaponLIndexX = randomIndex;
                break;
            }
            if (randomIndex == (weaponPoolL.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolL.Count) randomIndex++;
        }
        //�޵PY
        for (int i = 0; i < weaponPoolL.Count + 2; i++)
        {
            if (randomIndex != UsedWeaponLIndexX)
            {
                if (weaponPoolL[randomIndex] != CurrentWeaponL_weaponL)
                {
                    randomThreeWeaponLPool.Add(weaponPoolL[randomIndex]);
                    break;
                }
            }
            if (randomIndex == (weaponPoolL.Count - 1)) randomIndex = 0;
            if (randomIndex < weaponPoolL.Count) randomIndex++;
        }
        //�޵PZ
        foreach (var weaponZ in weaponPoolL)
        {
            if (weaponZ != randomThreeWeaponLPool[0] && weaponZ != randomThreeWeaponLPool[1])
            {

                randomThreeWeaponLPool.Add(weaponZ);
                break;
            }
        }
        return randomThreeWeaponLPool;
    }

    /// <summary>
    /// ��ܨèϥθӪZ��
    /// (�T��@����|�Ψ�o�Ө禡)
    /// </summary>
    public void ChooseAndUseWeapon(GameObject aWeapon)
    {
        int type = aWeapon.GetComponent<ItemOnWeapon>().weaponType;
        int id = aWeapon.GetComponent<ItemOnWeapon>().weaponID;

        WeaponSetActiveOpen(type, id);//�}�ҿ�ܪ��Z��

        if (type == 0)
            this.CurrentTorchL_torch = GetWeapon(type, id);//��e����
        if (type == 1)
            this.CurrentWeaponL_weaponL = GetWeapon(type, id);//��e����Z��
        UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//�]�m��e�Z���i�Z����
        if (type == 2)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//��e�k���Z��
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//�]�m��e�Z���i�Z����
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
        if (type == 3)
        {
            this.CurrentWeaponR_weaponR = GetWeapon(type, id);//��e�k����Z��
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetCurrentWeaponImage(aWeapon);//�]�m��e�Z���i�Z����
            player.GetComponent<PlayerController>().AutoSwitchWeaponR(this.CurrentWeaponR_weaponR);
        }
    }
    /*---------------------------------*/

    /// <summary>
    /// �M�����a��e�˳ƪ��Ҧ��Z��
    /// </summary>
    public void SetAllCurrentWeaponsEmpty()
    {
        CurrentTorchL_torch = null;
        CurrentWeaponL_weaponL = null;
        CurrentWeaponR_weaponR = null;
    }

}
