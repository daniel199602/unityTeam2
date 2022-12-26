using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �I��_�i�J�C��
    /// </summary>
    public void ClickPlayGameBtn()
    {
        UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetEmptyWeaponImage();//�M�ŪZ����ϥ�
        WeaponManager.Instance().SetAllCurrentWeaponsEmpty();//�M�����a��e�˳ƪ��Ҧ��Z��

        WeaponManager.Instance().SetDefaultWeaponFirst(); //�}�l�ɳ]�m�w�]�Z��(�C���i�ܥ�)

        GameManager.Instance().PlayerStart.SetActive(false);
        SceneManager.LoadSceneAsync("room");
        //SceneManager.LoadScene("room");
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// �I��_�����C��
    /// </summary>
    public void ClickQuitGameBtn()
    {
        GameManager.Instance().QuitGame();//�������ε{��
    }

}
