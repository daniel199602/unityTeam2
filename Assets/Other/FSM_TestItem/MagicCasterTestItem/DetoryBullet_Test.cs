using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetoryBullet_Test : MonoBehaviour
{
    public GameObject gg;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,2.5f);   
    }
    private void Update()
    {
        transform.position = gg.transform.position;
        transform.rotation = gg.transform.rotation;
    }
}
