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
    /// �I��_�i�J�C��
    /// </summary>
    public void ClickEnterGameBtn()
    {
        
    }

    /// <summary>
    /// �I��_���}�C��
    /// </summary>
    public void ClickQuitGameBtn()
    {
        GameManager.Instance().QuitGame();//�������ε{��
    }

}
