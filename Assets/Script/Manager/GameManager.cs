using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance;
    public static GameManager Instance() { return mInstance; }

    public List<GameObject> mobPool;
    private void Awake()
    {
        mInstance = this;
    }
    private void Start()
    {
        mobPool = new List<GameObject>();
    }

    private void Update()
    {
        
    }

}
