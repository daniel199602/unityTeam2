using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager mInstance;
    public static GameManager Instance() { return mInstance; }
    public GameObject PlayerCharacter = null;
    public List<GameObject> mobPool = new List<GameObject>();
    public GameObject fade = null;
    public GameObject MainCamera_Audio = null;
    public AudioClip[] audios;
    Animation fadeIn;
    [HideInInspector] public AudioSource audioSource;
    private float duration;

    [SerializeField] RectTransform canvasRoot = null;

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogWarning("有兩個相同的singleton物件,GameManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeIn = fade.GetComponent<Animation>();
        audioSource = MainCamera_Audio.GetComponent<AudioSource>();
        audioSource.clip = audios[0];
        audioSource.Play();
        PlayerSetActiveSwitch(false);//先將玩家關閉
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Boss"))
        {
            PlayerCharacter.SetActive(false);
            Debug.Log(PlayerCharacter.transform.position);
            audioSource.clip = audios[2];
            audioSource.Play();
            PlayerCharacter.transform.position = new Vector3(-195, 6, -330);
            Debug.Log(PlayerCharacter.transform.position);
            PlayerCharacter.SetActive(true);
            fadeIn.Play("FadeIn");
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("room"))
        {
            Debug.Log(PlayerCharacter.transform.position);
            PlayerCharacter.transform.position = new Vector3(-10, 6, -186.7f);
            audioSource.clip = audios[1];
            audioSource.Play();
            Debug.Log(PlayerCharacter.transform.position);
            PlayerCharacter.GetComponent<PlayerHpData>().HpAddToMax();//血量填滿
            PlayerCharacter.SetActive(true);
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
            PlayerCharacter.SetActive(false);
            SceneManager.LoadSceneAsync("room");
            PlayerCharacter.transform.position = new Vector3(0,2,-160);
            PlayerCharacter.SetActive(true);
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


    /// <summary>
    /// 玩家角色SetActive切換
    /// </summary>
    /// <param name="boolen">開關</param>
    public void PlayerSetActiveSwitch(bool boolen)
    {
        PlayerCharacter.SetActive(boolen);
    }

}
