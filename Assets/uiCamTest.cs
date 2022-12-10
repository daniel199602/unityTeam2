using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiCamTest : MonoBehaviour
{
    Vector3 camHp;
    //Quaternion originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        //originalRotation = transform.rotation;
        camHp = Camera.main.transform.forward;
        camHp.y = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(camHp);
        transform.rotation = Camera.main.transform.rotation;
        //transform.rotation = Camera.main.transform.rotation * originalRotation;

    }
}
