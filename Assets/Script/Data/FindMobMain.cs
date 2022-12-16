using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMobMain : MonoBehaviour
{

    // Start is called before the first frame update

    //private void Awake()
    //{
    //    GameManager.Instance().mobPool.Add(this.gameObject); 
    //}

    private void Start()
    {
        GameManager.Instance().mobPool.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
