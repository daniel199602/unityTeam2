using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuitGameUI : MonoBehaviour
{

    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// �I���T�w���}�ƥ�A�^�D���
    /// </summary>
    public void ClickQuitGameBtnYes()
    {
        SceneManager.LoadScene("GameMenu");
        UIManager.Instance().GameMenuPanelOpen();
        UIManager.Instance().QuitGameUIClose();
        GameManager.Instance().MobPoolClear();
    }

    /// <summary>
    /// �I���������}�ƥ�A�~��C��
    /// </summary>
    public void ClickQuitGameBtnNo()
    {
        UIManager.Instance().QuitGameUIClose();
    }

}
