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
    /// 點擊確定離開事件，離開遊戲
    /// </summary>
    public void ClickQuitGameBtnYes()
    {
        GameManager.Instance().QuitGame();//關閉應用程式
    }

    /// <summary>
    /// 點擊取消離開事件，繼續遊戲
    /// </summary>
    public void ClickQuitGameBtnNo()
    {
        UIManager.Instance().QuitGameUIClose();
    }

}
