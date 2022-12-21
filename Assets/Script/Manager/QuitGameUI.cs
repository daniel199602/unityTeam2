using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuitGameUI : MonoBehaviour
{

    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// �I���T�w���}�ƥ�A���}�C��
    /// </summary>
    public void ClickQuitGameBtnYes()
    {
        GameManager.Instance().QuitGame();//�������ε{��
    }

    /// <summary>
    /// �I���������}�ƥ�A�~��C��
    /// </summary>
    public void ClickQuitGameBtnNo()
    {
        UIManager.Instance().QuitGameUIClose();
    }

}
