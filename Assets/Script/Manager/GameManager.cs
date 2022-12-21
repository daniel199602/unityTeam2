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
    
    private float duration;

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
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
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Boss"))
        {
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.transform.position = new Vector3(-195, 2, -330);
            Debug.Log(PlayerStart.transform.position);
            PlayerStart.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerStart.SetActive(false);
            SceneManager.LoadScene("room");
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

}
