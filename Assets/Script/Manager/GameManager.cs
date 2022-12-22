using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;
    public static GameManager Instance() { return mInstance; }
    public GameObject PlayerStart;
    public List<GameObject> mobPool;
    public GameObject fade;
    Animation fadeIn;

    private float duration;

    private void Awake()
    {
        if (mInstance != null)
        {
            //Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            Debug.LogWarning("有兩個相同的singleton物件,GameManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
        
        mobPool = new List<GameObject>();
        PlayerStart = GameObject.FindWithTag("Player");
        

        //mobPool.Add(PlayerStart);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeIn = fade.GetComponent<Animation>();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Boss"))
        {
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.transform.position = new Vector3(-195, 2, -330);
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.SetActive(true);
            fadeIn.Play("FadeIn");
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("room"))
        {
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.transform.position = new Vector3(-10, 10, -186.7f);
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.SetActive(true);
            fadeIn.Play("FadeIn");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MobPoolClear();
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetEmptyWeaponImage();//清空武器格圖示
            WeaponManager.Instance().SetAllCurrentWeaponsEmpty();//清除玩家當前裝備的所有武器
            PlayerStart.SetActive(false);
            SceneManager.LoadSceneAsync("room");
            PlayerStart.transform.position = new Vector3(0,2,-160);
            PlayerStart.SetActive(true);
        }
    }

    /// <summary>
    /// 直接關閉應用程式
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); //關閉應用程式
    }

    /// <summary>
    /// 清除怪物池
    /// </summary>
    public void MobPoolClear()
    {
        mobPool.Clear();
    }

}
