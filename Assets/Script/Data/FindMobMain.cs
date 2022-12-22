using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMobMain : MonoBehaviour
{

    private void Awake()
    {
        //GameManager.Instance().mobPool.Add(this.gameObject);
    }

    private void Start()
    {
        //GameManager.Instance().mobPool.Clear();
         GameManager.Instance().mobPool.Add(this.gameObject);
    }

}
