using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetoryBullet_Test : MonoBehaviour
{
     GameObject gg;
    // Start is called before the first frame update
    void Start()
    {
        gg = GameObject.Find("DemonGirlMesh_C");
        Destroy(this.gameObject,2.5f);   
    }
    private void Update()
    {
        transform.position = gg.transform.position;
        transform.rotation = gg.transform.rotation;
    }
}
