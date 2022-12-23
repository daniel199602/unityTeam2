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
            Debug.LogWarning("����ӬۦP��singleton����,GameManager");
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
        
        mobPool = new List<GameObject>();
        PlayerStart = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        fadeIn = fade.GetComponent<Animation>();

        PlayerSetActiveSwitch(false);//���N���a����
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
            UIManager.Instance().weaponFramePanel.GetComponent<WeaponFrameUI>().SetEmptyWeaponImage();//�M�ŪZ����ϥ�
            WeaponManager.Instance().SetAllCurrentWeaponsEmpty();//�M�����a��e�˳ƪ��Ҧ��Z��
            PlayerStart.SetActive(false);
            SceneManager.LoadSceneAsync("room");
            PlayerStart.transform.position = new Vector3(0,2,-160);
            PlayerStart.SetActive(true);
        }
    }

    /// <summary>
    /// �����������ε{��
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); //�������ε{��
    }

    /// <summary>
    /// �M���Ǫ���
    /// </summary>
    public void MobPoolClear()
    {
        mobPool.Clear();
    }


    /// <summary>
    /// ���a����SetActive����
    /// </summary>
    /// <param name="boolen">�}��</param>
    public void PlayerSetActiveSwitch(bool boolen)
    {
        PlayerStart.SetActive(boolen);
    }

}
