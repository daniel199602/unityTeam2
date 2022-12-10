using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiCamTest : MonoBehaviour
{
    Vector3 camHp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camHp = Camera.main.transform.forward;
        camHp.y = 0;
        transform.rotation = Quaternion.LookRotation(camHp);
        
        
    }
}
