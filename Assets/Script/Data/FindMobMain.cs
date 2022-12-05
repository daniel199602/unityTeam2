using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMobMain : MonoBehaviour
{

    // Start is called before the first frame update

    private void Start()
    {
        GameObject go = GameObject.Find("MobMain");
        this.transform.SetParent(go.transform);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
