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
    /// 點擊確定離開事件，回主選單
    /// </summary>
    public void ClickQuitGameBtnYes()
    {
        SceneManager.LoadScene("GameMenu");
        UIManager.Instance().GameMenuPanelOpen();
        UIManager.Instance().QuitGameUIClose();
        GameManager.Instance().MobPoolClear();
        GameManager.Instance().PlayerSetActiveSwitch(false);
        GameManager.Instance().audioSource.clip = GameManager.Instance().audios[0];
        GameManager.Instance().audioSource.Play();
    }

    /// <summary>
    /// 點擊取消離開事件，繼續遊戲
    /// </summary>
    public void ClickQuitGameBtnNo()
    {
        UIManager.Instance().QuitGameUIClose();
    }

}
