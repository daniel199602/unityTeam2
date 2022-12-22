using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void ClickEnterGameBtn()
    {
        
    }

    /// <summary>
    /// 點擊_離開遊戲
    /// </summary>
    public void ClickQuitGameBtn()
    {
        GameManager.Instance().QuitGame();//關閉應用程式
    }

}
