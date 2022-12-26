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
    /// 點擊_進入遊戲
    /// </summary>
    public void ClickPlayGameBtn()
    {
        UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetEmptyWeaponImage();//清空武器格圖示
        WeaponManager.Instance().SetAllCurrentWeaponsEmpty();//清除玩家當前裝備的所有武器

        WeaponManager.Instance().SetDefaultWeaponFirst(); //開始時設置預設武器(遊戲展示用)

        GameManager.Instance().PlayerStart.SetActive(false);
        SceneManager.LoadSceneAsync("room");
        //SceneManager.LoadScene("room");
        transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// 點擊_結束遊戲
    /// </summary>
    public void ClickQuitGameBtn()
    {
        GameManager.Instance().QuitGame();//關閉應用程式
    }

}
