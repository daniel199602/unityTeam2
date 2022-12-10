using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;
    public static GameManager Instance() { return mInstance; }
    GameObject PlayerStart;
    public List<GameObject> mobPool;
    private void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
        mobPool = new List<GameObject>();
        PlayerStart = GameObject.FindWithTag("Player");
        mobPool.Add(PlayerStart);
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

}
